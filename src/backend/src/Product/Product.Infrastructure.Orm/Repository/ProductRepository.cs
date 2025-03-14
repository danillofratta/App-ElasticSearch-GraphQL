﻿
using Product.Core.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;
using Shared.Infrastructure.Orm;
using Shared.Infrasctructure.Orm.Service;

namespace Product.Infrastructure.Orm.Repository;

public class ProductRepository : RepositoryBase<ProductCoreDomainEntities.Product>, IProductRepository
{
    public ProductRepository(DefaultDbContext defaultDbContext, IRedisCacheService redisCacheService) : base(defaultDbContext, redisCacheService)
    {
    }

    private async Task AddCache(ProductCoreDomainEntities.Product obj)
    {
        await _RedisCacheService.SetAsync($"product:{obj.Id}", obj, TimeSpan.FromHours(1));        
    }

    public async Task<(IReadOnlyList<ProductCoreDomainEntities.Product> products, int totalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string orderBy,
    bool isDescending,
    CancellationToken cancellationToken)
    {
        IQueryable<ProductCoreDomainEntities.Product> query = _DefaultDbContext.Products.AsQueryable();

        // Apply ordering if specified
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(s => s.Name)
                    : query.OrderBy(s => s.Name),
                "price" => isDescending
                    ? query.OrderByDescending(s => s.Price)
                    : query.OrderBy(s => s.Price),
                "date" => isDescending
                    ? query.OrderByDescending(s => s.CreatedAt)
                    : query.OrderBy(s => s.CreatedAt),
                _ => query.OrderByDescending(s => s.CreatedAt) // Default ordering
            };
        }
        else
        {
            // Default ordering if none specified
            query = query.OrderByDescending(s => s.CreatedAt);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    Task IProductRepository.AfterDeleteAsync(ProductCoreDomainEntities.Product obj)
    {
        throw new NotImplementedException();
    }

    public override async Task AfterSaveAsync(ProductCoreDomainEntities.Product obj)
    {
        await this.AddCache(obj);
        await _RedisCacheService.AddToSetAsync("all:products", obj.Id.ToString());
        base.AfterSaveAsync(obj);
    }

    public override async Task AfterUpdateAsync(ProductCoreDomainEntities.Product obj)
    {
        await this.AddCache(obj);

        base.AfterUpdateAsync(obj);
    }

    public override async Task BeforeUpdateAsync(ProductCoreDomainEntities.Product obj)
    {
        await _RedisCacheService.RemoveAsync($"product:{obj.Id}");
        obj.UpdatedAt = DateTime.UtcNow;

        base.BeforeUpdateAsync(obj);
    }

    public override async Task AfterDeleteAsync(ProductCoreDomainEntities.Product obj)
    {
        await _RedisCacheService.RemoveAsync($"product:{obj.Id}");
        await _RedisCacheService.RemoveFromSetAsync("all:products", obj.Id.ToString());


        base.AfterDeleteAsync(obj);
    }

    public async Task<List<ProductCoreDomainEntities.Product>> GetAllAsync()
    {
        return await _DefaultDbContext.Products.ToListAsync();
    }

    public async Task<ProductCoreDomainEntities.Product> GetByIdAsync(Guid id)
    {        
        var cachedProduct = await _RedisCacheService.GetAsync<ProductCoreDomainEntities.Product>($"product:{id}");
        if (cachedProduct != null)
        {
            return cachedProduct;
        }

        var product =  await _DefaultDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
        await this.AddCache(product);

        return product;
    }

    public async Task<List<ProductCoreDomainEntities.Product>> GetByName(string name)
    {
        var productIds = await _RedisCacheService.GetSetMembersAsync("all:products");

        List<ProductCoreDomainEntities.Product> products = new List<ProductCoreDomainEntities.Product>();

        if (productIds.Any())
        {
            foreach (var id in productIds)
            {
                var product = await _RedisCacheService.GetAsync<ProductCoreDomainEntities.Product>($"product:{id}");
                if (product != null && product.Name.ToUpper().Contains(name.ToUpper()))
                    products.Add(product);
            }
        }

        if (!products.Any())
        {
            products = await _DefaultDbContext.Products.Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToListAsync();

            foreach (var product in products)
            {                
                this.AddCache(product);
                await _RedisCacheService.AddToSetAsync("all:products", product.Id.ToString());
            }
        }

        return products;
    }
}

using Base.Core.Domain.Common;

namespace Product.Core.Domain.Repository;

public interface IProductRepository : IRepositoryBase<ProductCoreDomainEntities.Product>
{
    Task<ProductCoreDomainEntities.Product> GetByIdAsync(Guid id);

    Task<List<ProductCoreDomainEntities.Product>> GetAllAsync();

    Task<List<ProductCoreDomainEntities.Product>> GetByName(string name);

    Task AfterSaveAsync(ProductCoreDomainEntities.Product obj);

    Task AfterUpdateAsync(ProductCoreDomainEntities.Product obj);

    Task BeforeUpdateAsync(ProductCoreDomainEntities.Product obj);

    Task AfterDeleteAsync(ProductCoreDomainEntities.Product obj);

    Task<(IReadOnlyList<ProductCoreDomainEntities.Product> products, int totalCount)> GetPagedAsync
        (
        int pageNumber,
        int pageSize,
        string orderBy,
        bool isDescending,
        CancellationToken cancellationToken
        );
}

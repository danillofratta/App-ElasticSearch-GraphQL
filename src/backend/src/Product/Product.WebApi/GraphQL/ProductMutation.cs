using HotChocolate;
using MediatR;
using Nest;
using Product.Core.Application.Product.Event.Create;
using Product.Core.Application.Product.Repository.Command.Create;
using Product.Core.Application.Product.Repository.Command.Delete;
using Product.Core.Application.Product.Repository.Command.Modify;

namespace Product.WebApi.GraphQL;

public class ProductMutation
{
    private readonly IMediator _mediator;
    private readonly IElasticClient _elasticClient;

    public ProductMutation(IMediator mediator, IElasticClient elasticClient)
    {
        _mediator = mediator;
        _elasticClient = elasticClient;
    }

    public async Task<ProductDocument> CreateProduct(string name, decimal price)
    {
        // Send the command to create the product (write side)
        var command = new CreateProductCommand { Name = name, Price = price };
        var result = await _mediator.Send(command);

        // Query Elasticsearch to get the ProductDocument (read side)
        var productDoc = await _elasticClient.GetAsync<ProductDocument>(result.Id);
        if (!productDoc.IsValid || !productDoc.Found)
            throw new System.Collections.Generic.KeyNotFoundException($"Product with ID {result.Id} not found in Elasticsearch.");

        return productDoc.Source;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        // Send the command to delete the product (write side)
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);

        // Optionally, verify deletion in Elasticsearch (not strictly necessary but good for consistency)
        var response = await _elasticClient.GetAsync<ProductDocument>(id);
        return !response.Found; // Return true if the product is no longer found
    }

    public async Task<ProductDocument> UpdateProduct(Guid id, string name, decimal price)
    {
        // Send the command to update the product (write side)
        var command = new ModifyProductCommand { Id = id, Name = name, Price = price };
        await _mediator.Send(command);

        // Query Elasticsearch to get the updated ProductDocument (read side)
        var productDoc = await _elasticClient.GetAsync<ProductDocument>(id);
        if (!productDoc.IsValid || !productDoc.Found)
            throw new System.Collections.Generic.KeyNotFoundException($"Product with ID {id} not found in Elasticsearch.");

        return productDoc.Source;
    }
}

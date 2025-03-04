using MediatR;
using Nest;
using Product.Core.Application.Product.Elastic.EventElastic.Create;
using Product.Core.Application.Product.Event.Create;

namespace Product.Core.Application.Product.Event.Delete;

public class ProductDeleteEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IElasticClient _elasticClient;

    public ProductDeleteEventHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var productDoc = new ProductDocument
        {
            Id = notification.Id
        };
        await _elasticClient.DeleteAsync<ProductDocument>(productDoc);
    }
}


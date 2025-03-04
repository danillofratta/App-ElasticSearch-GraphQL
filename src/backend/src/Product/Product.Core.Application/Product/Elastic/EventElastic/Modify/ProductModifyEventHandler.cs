using MediatR;
using Nest;
using Product.Core.Application.Product.Elastic.EventElastic.Create;
using Product.Core.Application.Product.Event.Create;

namespace Product.Core.Application.Product.Event.Modify;

public class ProductModifyEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IElasticClient _elasticClient;

    public ProductModifyEventHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var productDoc = new ProductDocument
        {
            Id = notification.Id,
            Name = notification.Name,
            Price = notification.Price
        };
        await _elasticClient.IndexDocumentAsync(productDoc);
    }
}


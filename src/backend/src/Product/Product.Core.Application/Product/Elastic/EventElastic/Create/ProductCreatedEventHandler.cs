using MediatR;
using Nest;
using Product.Core.Application.Product.Event.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Application.Product.Elastic.EventElastic.Create;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IElasticClient _elasticClient;

    public ProductCreatedEventHandler(IElasticClient elasticClient)
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

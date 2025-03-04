using MediatR;
using Nest;
using Product.Core.Application.Product.Event.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Application.Product.Elastic.Query.GetById;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDocument>
{
    private readonly IElasticClient _elasticClient;

    public GetProductByIdQueryHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<ProductDocument> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.GetAsync<ProductDocument>(request.Id);

        if (!response.IsValid || !response.Found)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

        return response.Source;
    }
}
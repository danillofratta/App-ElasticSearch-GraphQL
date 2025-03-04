using MediatR;
using Nest;
using Product.Core.Application.Product.Event.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Application.Product.Elastic.Query.GetByName;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IReadOnlyList<ProductDocument>>
{
    private readonly IElasticClient _elasticClient;

    public GetProductByNameQueryHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IReadOnlyList<ProductDocument>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = RemoveSpecialCharacters(request.Name.ToLower());

        var response = await _elasticClient.SearchAsync<ProductDocument>(s => s
            .Query(q => q
                .QueryString(qs => qs
                    .Fields(f => f.Field(p => p.Name))
                    .Query($"*{searchTerm}*")                    
                )
            )
        ); 

        return response.Documents.ToList();
    }

    private string RemoveSpecialCharacters(string input)
    {
        return new string(input
            .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
            .ToArray());
    }
}
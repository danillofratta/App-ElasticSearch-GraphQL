using MediatR;
using Nest;
using Product.Core.Application.Product.Event.Create;

namespace Product.Core.Application.Product.Elastic.Query.GetAll;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IReadOnlyList<ProductDocument>>
{
    private readonly IElasticClient _elasticClient;

    public GetAllProductQueryHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<IReadOnlyList<ProductDocument>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<ProductDocument>(s => s
            .Query(q => q
                .MatchAll()
            )
            .Size(1000) // Adjust size based on your needs
        );

        if (!response.IsValid)
        {
            throw new Exception($"Erro na busca: {response.OriginalException?.Message}");
        }

        return response.Documents.ToList();
    }
}
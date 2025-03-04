using MediatR;

namespace Product.Core.Application.Product.Query.GetByName
{
    public class GetProductByNameQuery : IRequest<List<GetProductByNameQueryResult>>
    {
        public string Name { get; set; } = string.Empty;

        public GetProductByNameQuery(string name)
        {
            Name = name;
        }

    }
}

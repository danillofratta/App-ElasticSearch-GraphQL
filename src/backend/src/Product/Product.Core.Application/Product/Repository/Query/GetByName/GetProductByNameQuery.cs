using MediatR;

namespace Product.Core.Application.Product.Repository.Query.GetName
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

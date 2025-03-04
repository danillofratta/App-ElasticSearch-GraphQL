using MediatR;

namespace Product.Core.Application.Product.Repository.Query.Get
{
    public class GetProductByIdQuery : IRequest<GetProductByIdQueryResult>
    {
        public Guid Id { get; set; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}

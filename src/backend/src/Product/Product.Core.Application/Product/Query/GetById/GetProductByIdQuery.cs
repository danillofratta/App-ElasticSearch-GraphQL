using MediatR;

namespace Product.Core.Application.Product.Query.GetById
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

using MediatR;

namespace Product.Core.Application.Product.Repository.Command.Delete
{
    public class DeleteProductCommand : IRequest<DeleteProductResult>
    {
        public Guid Id { get; set; }
    }
}

using MediatR;

namespace Product.Core.Application.Product.Repository.Command.Create
{
    public class CreateProductCommand : IRequest<CreateProductResult>
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}

using MediatR;

namespace Product.Core.Application.Product.Command.Modify
{
    public class ModifyProductCommand : IRequest<ModifyProductResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

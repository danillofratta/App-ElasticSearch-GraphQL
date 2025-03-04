using MediatR;


namespace Product.Core.Application.Product.Command.Modify
{
    public class ModifyProductResult : INotification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

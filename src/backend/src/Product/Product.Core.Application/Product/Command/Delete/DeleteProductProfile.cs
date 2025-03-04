using AutoMapper;

namespace Product.Core.Application.Product.Command.Delete
{
    public class DeleteProductProfile : Profile
    {
        public DeleteProductProfile()
        {
            CreateMap<DeleteProductCommand, ProductCoreDomainEntities.Product>();
            CreateMap<ProductCoreDomainEntities.Product, DeleteProductResult>();
        }
    }
}

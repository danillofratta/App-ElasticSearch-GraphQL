using AutoMapper;

namespace Product.Core.Application.Product.Repository.Command.Delete
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

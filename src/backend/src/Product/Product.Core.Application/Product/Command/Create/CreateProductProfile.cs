using AutoMapper;


namespace Product.Core.Application.Product.Command.Create
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {

            CreateMap<CreateProductCommand, ProductCoreDomainEntities.Product>();

            CreateMap<ProductCoreDomainEntities.Product, CreateProductResult>();
        }
    }
}

using AutoMapper;


namespace Product.Core.Application.Product.Query.GetByName
{
    public class GetProductByNameQueryProfile : Profile
    {
        public GetProductByNameQueryProfile()
        {
            CreateMap<ProductCoreDomainEntities.Product, GetProductByNameQueryResult>();
        }
    }
}

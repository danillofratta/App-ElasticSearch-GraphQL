using AutoMapper;


namespace Product.Core.Application.Product.Repository.Query.GetName
{
    public class GetProductByNameQueryProfile : Profile
    {
        public GetProductByNameQueryProfile()
        {
            CreateMap<ProductCoreDomainEntities.Product, GetProductByNameQueryResult>();
        }
    }
}

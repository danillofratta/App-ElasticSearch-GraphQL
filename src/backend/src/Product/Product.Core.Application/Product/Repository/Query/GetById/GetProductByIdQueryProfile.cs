using AutoMapper;


namespace Product.Core.Application.Product.Repository.Query.Get
{
    public class GetProductByIdQueryProfile : Profile
    {
        public GetProductByIdQueryProfile()
        {
            CreateMap<ProductCoreDomainEntities.Product, GetProductByIdQueryResult>();
        }
    }
}

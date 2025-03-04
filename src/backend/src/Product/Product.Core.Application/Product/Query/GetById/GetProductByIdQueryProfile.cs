using AutoMapper;


namespace Product.Core.Application.Product.Query.GetById
{
    public class GetProductByIdQueryProfile : Profile
    {
        public GetProductByIdQueryProfile()
        {
            CreateMap<ProductCoreDomainEntities.Product, GetProductByIdQueryResult>();
        }
    }
}

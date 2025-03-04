using AutoMapper;

namespace Product.Core.Application.Product.Query.GetAll
{
    public class GetAllProductQueryProfile : Profile
    {
        public GetAllProductQueryProfile()
        {
            CreateMap<ProductCoreDomainEntities.Product, GetAllProductQueryResult>();
        }
    }
}

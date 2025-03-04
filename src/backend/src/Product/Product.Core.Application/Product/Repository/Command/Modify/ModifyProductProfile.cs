﻿using AutoMapper;

namespace Product.Core.Application.Product.Repository.Command.Modify
{
    public class ModifyProductProfile : Profile
    {
        public ModifyProductProfile()
        {
            CreateMap<ModifyProductCommand, ProductCoreDomainEntities.Product>();
            CreateMap<ProductCoreDomainEntities.Product, ModifyProductResult>();
        }
    }
}

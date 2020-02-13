using AutoMapper;
using ProductCatalogue.Contacts;
using ProductCatalogue.Models;
using ProductCatalogue.Repository.EfModel;
using System;

namespace ProductCatalogueApi
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductModel, ProductContract>();
            CreateMap<ProductContract, ProductModel>();
            CreateMap<Product, ProductContract>();
            CreateMap<ProductContract, Product>();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;

namespace ProductCatalogue.AggregateRoute
{
    public class AddProduct : IAddProduct
    {
        IProductRepository _prdRepo;
        public AddProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public BaseResponse Execute(ProductContract obj)
        {
            return new BaseResponse
            {
                Message = "Product Added Successfully",
                Success = _prdRepo.AddProduct(obj)
            };
        }
    }
}

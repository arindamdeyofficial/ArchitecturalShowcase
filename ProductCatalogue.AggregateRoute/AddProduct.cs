using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Threading.Tasks;

namespace ProductCatalogue.AggregateRoute
{
    public class AddProduct : IAddProduct
    {
        IProductRepository _prdRepo;
        public AddProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public async Task<BaseResponse> Execute(ProductContract obj)
        {
            return new BaseResponse
            {
                Message = "Product Added Successfully",
                Success = await _prdRepo.AddProduct(obj)
            };
        }
    }
}

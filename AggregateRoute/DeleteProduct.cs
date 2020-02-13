using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Threading.Tasks;

namespace ProductCatalogue.AggregateRoute
{    
    public class DeleteProduct : IDeleteProduct
    {
        IProductRepository _prdRepo;
        public DeleteProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public async Task<BaseResponse> Execute(ProductContract obj)
        {            
            return new BaseResponse
            {
                Message = "Product Added Successfully",
                Success = await _prdRepo.DeleteProduct(obj)
        };
        }
    }
}

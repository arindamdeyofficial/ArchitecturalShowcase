using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;

namespace ProductCatalogue.AggregateRoute
{    
    public class DeleteProduct : IDeleteProduct
    {
        IProductRepository _prdRepo;
        public DeleteProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public BaseResponse Execute(ProductContract obj)
        {            
            return new BaseResponse
            {
                Message = "Product Added Successfully",
                Success = _prdRepo.DeleteProduct(obj)
        };
        }
    }
}

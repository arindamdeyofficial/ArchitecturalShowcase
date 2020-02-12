using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Threading.Tasks;

namespace ProductCatalogue.AggregateRoute
{
    public class UpdateProduct : IUpdateProduct
    {
        IProductRepository _prdRepo;
        public UpdateProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public async Task<BaseResponse> Execute(ProductContract obj)
        {
            return new BaseResponse
            {
                Message = "Product Updated Successfully",
                Success = await _prdRepo.UpdateProduct(obj)
            };
        }
    }
}

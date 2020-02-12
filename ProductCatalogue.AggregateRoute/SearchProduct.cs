using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogue.AggregateRoute
{
    public class SearchProduct : ISearchProduct
    {
        IProductRepository _prdRepo;
        public SearchProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public async Task<BaseResponse> Execute(ProductContract obj)
        {
            return await _prdRepo.SearchProduct(obj);
        }
    }
}

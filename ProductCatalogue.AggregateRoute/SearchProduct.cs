using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Collections.Generic;

namespace ProductCatalogue.AggregateRoute
{
    public class SearchProduct : ISearchProduct
    {
        IProductRepository _prdRepo;
        public SearchProduct([FromServices] IProductRepository prdRepo)
        {
            _prdRepo = prdRepo;
        }
        public BaseResponse Execute(ProductContract obj)
        {
            return _prdRepo.SearchProduct(obj);
        }
    }
}

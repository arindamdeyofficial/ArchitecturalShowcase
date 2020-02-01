using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.AggregateRoute;
using System;

namespace ProductCatalogue.Contacts.ServiceContracts
{
    public interface IProductCatalogueServiceFacade
    {
        BaseResponse AddProduct(ProductContract prd, IAddProduct exec);
        BaseResponse DeleteProduct(ProductContract prd, IDeleteProduct exec);
        BaseResponse UpdateProduct(ProductContract prd, IUpdateProduct exec);
        BaseResponse SearchProduct(ProductContract prd, ISearchProduct exec);
    }
}

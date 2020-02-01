using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Models;
using System;

namespace ProductCatalogue.Facade
{
    public class ProductCatalogueServiceFacade: IProductCatalogueServiceFacade
    {
        
        private static readonly Lazy<ProductCatalogueServiceFacade> _instance = new Lazy<ProductCatalogueServiceFacade>(() => new ProductCatalogueServiceFacade());
        public static ProductCatalogueServiceFacade Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public BaseResponse AddProduct(ProductContract prd, IAddProduct exec)
        {
            return exec.Execute(prd);
        }
        public BaseResponse DeleteProduct(ProductContract prd, IDeleteProduct exec)
        {
            return exec.Execute(prd);
        }
        public BaseResponse UpdateProduct(ProductContract prd, IUpdateProduct exec)
        {
            return exec.Execute(prd);
        }
        public BaseResponse SearchProduct(ProductContract prd, ISearchProduct exec)
        {
            return exec.Execute(prd);
        }
    }
}

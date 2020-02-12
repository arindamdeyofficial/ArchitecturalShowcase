using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.AggregateRoute;
using System;
using System.Threading.Tasks;

namespace ProductCatalogue.Contacts.ServiceContracts
{
    public interface IProductCatalogueServiceFacade
    {
        Task<BaseResponse> AddProduct(ProductContract prd, IAddProduct exec);
        Task<BaseResponse> DeleteProduct(ProductContract prd, IDeleteProduct exec);
        Task<BaseResponse> UpdateProduct(ProductContract prd, IUpdateProduct exec);
        Task<BaseResponse> SearchProduct(ProductContract prd, ISearchProduct exec);
    }
}

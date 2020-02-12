using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogue.Contacts.ServiceContracts
{
    public interface IProductRepository
    {
        Task<bool> AddProduct(ProductContract prd);
        Task<bool> DeleteProduct(ProductContract prd);
        Task<ProductSearchResponse> SearchProduct(ProductContract prd);
        Task<bool> UpdateProduct(ProductContract prd);
    }
}

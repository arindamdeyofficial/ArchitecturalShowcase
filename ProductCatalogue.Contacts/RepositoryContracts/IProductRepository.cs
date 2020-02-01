using System;
using System.Collections.Generic;

namespace ProductCatalogue.Contacts.ServiceContracts
{
    public interface IProductRepository
    {
        bool AddProduct(ProductContract prd);
        bool DeleteProduct(ProductContract prd);
        ProductSearchResponse SearchProduct(ProductContract prd);
        bool UpdateProduct(ProductContract prd);
    }
}

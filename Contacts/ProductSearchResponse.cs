using System;
using System.Collections.Generic;

namespace ProductCatalogue.Contacts
{
    public class ProductSearchResponse:BaseResponse
    {
        public List<ProductContract> Products { get; set; }
    }
}

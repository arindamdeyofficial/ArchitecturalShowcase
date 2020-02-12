using System;

namespace ProductCatalogue.Models
{
    public class ProductModel
    {
        public string Title { get; set; }
        public string BusinessName { get; set; }
        public decimal Cost { get; set; }

        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}

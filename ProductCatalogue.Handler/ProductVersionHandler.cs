using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using System;

namespace ProductCatalogue.Handler
{
    //Product Catalogue multiple version implementation
    public class ProductVersionHandler
    {
        static ServiceCollection srvCollection = new ServiceCollection();
        private ProductVersionHandler()
        {

        }
        private static readonly Lazy<ProductVersionHandler> _instance = new Lazy<ProductVersionHandler>(() => new ProductVersionHandler());
        public static ProductVersionHandler Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        static IAggregateRoute _prdAr;
        //static BaseResponse AddProduct([FromServices])
        public static BaseResponse AddProduct(int i)
        {
            srvCollection.GetService
        }
    }
}

using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using System;
using System.Threading.Tasks;

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
        public async Task<BaseResponse> AddProduct(ProductContract prd, IAddProduct exec)
        {
            return await exec.Execute(prd);
        }
        public async Task<BaseResponse> DeleteProduct(ProductContract prd, IDeleteProduct exec)
        {
            return await exec.Execute(prd);
        }
        public async Task<BaseResponse> UpdateProduct(ProductContract prd, IUpdateProduct exec)
        {
            return await exec.Execute(prd);
        }
        public async Task<BaseResponse> SearchProduct(ProductContract prd, ISearchProduct exec)
        {
            return await exec.Execute(prd);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Models;
using System.Threading.Tasks;

namespace ProductCatalogueApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductCatalogueController : BaseController<ProductCatalogueController>
    {
        public ProductCatalogueController(
           IMapper mapper,
           [FromServices]IProductCatalogueServiceFacade productCatalogueServiceFacade,
           IOptions<AppConfigs> configs):base(mapper, productCatalogueServiceFacade, configs) { }

        [HttpGet("~/api/[controller]/SearchProduct")]
        public async Task<JsonResult> SearchProduct(string title, [FromServices]ISearchProduct fsd)
        {
            var r = await _productCatalogueServiceFacade.SearchProduct(new ProductContract
            {
                Title = title
            }, fsd);
            return new JsonResult(r);
        }

        [HttpPost]
        [Route("~/api/[controller]/AddProduct")]
        public async Task<JsonResult> AddProductAsync(ProductModel prd, [FromServices]IAddProduct fsd)
        {

            return new JsonResult(await _productCatalogueServiceFacade.AddProduct(_mapper.Map<ProductContract>(prd), fsd));
        }

        // PUT api/<controller>/5
        [HttpPut("~/api/[controller]/UpdateProduct")]
        public async Task<JsonResult> UpdateProduct(ProductModel prd, [FromServices]IUpdateProduct fsd)
        {
            return new JsonResult(await _productCatalogueServiceFacade.UpdateProduct(_mapper.Map<ProductContract>(prd), fsd));
        }

        // DELETE api/<controller>/5
        [HttpDelete("~/api/[controller]/DeleteProduct")]
        public async Task<JsonResult> DeleteProduct(string title, [FromServices]IDeleteProduct fsd)
        {
            return new JsonResult(await _productCatalogueServiceFacade.DeleteProduct(new ProductContract
            {
                Title = title
            }, fsd));
        }
    }
}

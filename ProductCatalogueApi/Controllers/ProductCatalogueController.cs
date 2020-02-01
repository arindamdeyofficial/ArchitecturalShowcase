using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogueApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductCatalogueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductCatalogueServiceFacade _productCatalogueServiceFacade;
        public ProductCatalogueController(IMapper mapper, [FromServices]IProductCatalogueServiceFacade productCatalogueServiceFacade)
        {
            _mapper = mapper;

            _productCatalogueServiceFacade = productCatalogueServiceFacade;
        }

        [HttpGet("~/api/[controller]/SearchProduct")]
        public JsonResult SearchProduct(string title, [FromServices]ISearchProduct fsd)
        {
            return new JsonResult(_productCatalogueServiceFacade.SearchProduct(new ProductContract
            {
                Title = title
            }, fsd));
        }

        [HttpPost]
        [Route("~/api/[controller]/AddProduct")]
        public JsonResult AddProduct(ProductModel prd, [FromServices]IAddProduct fsd)
        {

            return new JsonResult(_productCatalogueServiceFacade.AddProduct(_mapper.Map<ProductContract>(prd), fsd));
        }

        // PUT api/<controller>/5
        [HttpPut("~/api/[controller]/UpdateProduct")]
        public JsonResult UpdateProduct(ProductModel prd, [FromServices]IUpdateProduct fsd)
        {
            return new JsonResult(_productCatalogueServiceFacade.UpdateProduct(_mapper.Map<ProductContract>(prd), fsd));
        }

        // DELETE api/<controller>/5
        [HttpDelete("~/api/[controller]/DeleteProduct")]
        public JsonResult DeleteProduct(string title, [FromServices]IDeleteProduct fsd)
        {
            return new JsonResult(_productCatalogueServiceFacade.DeleteProduct(new ProductContract
            {
                Title = title
            }, fsd));
        }
    }
}

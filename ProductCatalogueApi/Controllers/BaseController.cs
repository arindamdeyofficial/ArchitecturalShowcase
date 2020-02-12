using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogueApi.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        protected IMapper _mapper;
        protected IProductCatalogueServiceFacade _productCatalogueServiceFacade;
        protected AppConfigs _configs;
        protected BaseController(
            IMapper mapper, 
            [FromServices]IProductCatalogueServiceFacade productCatalogueServiceFacade, 
            IOptions<AppConfigs> configs)
        {
            _mapper = mapper;
            _configs = configs.Value;
            _productCatalogueServiceFacade = productCatalogueServiceFacade;
        }
        protected BaseController()
        {
        }
    }
}

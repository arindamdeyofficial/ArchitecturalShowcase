using AutoMapper;
using Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCatalogue.Contacts;

namespace ProductCatalogueApi.Controllers
{
    [Route("api/[controller]")]
    public class DesignPatternController : BaseController<DesignPatternController>
    {
        public readonly IFactoryPattern _factory;
        public DesignPatternController(
           IMapper mapper, IOptions<AppConfigs> configs, IFactoryPattern factory)
            :base(mapper, null, configs) 
        {
            _factory = factory;
        }

        [HttpGet("~/api/[controller]/Factory")]
        public JsonResult ExucuteFactory(string str)
        {
            return new JsonResult(_factory.CreateDevice(str));
        }
    }
}

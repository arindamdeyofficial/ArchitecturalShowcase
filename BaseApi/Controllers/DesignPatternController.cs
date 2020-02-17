using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatternCaller;
using ProductCatalogue.Contacts;

namespace ProductCatalogueApi.Controllers
{
    [Route("api/[controller]")]
    public class DesignPatternController : BaseController<DesignPatternController>
    {
        public readonly IPatterns _patterns;
        public DesignPatternController(
           IMapper mapper, IOptions<AppConfigs> configs, IPatterns patterns)
            :base(mapper, null, configs) 
        {
            _patterns = patterns;
        }

        [HttpGet("~/api/[controller]/Factory")]
        public JsonResult ExucuteFactory(string str)
        {
            //return new JsonResult(_patterns.CreateDevice(str));
            return new JsonResult(true);
        }
    }
}

using AutoMapper;
using Helpers;
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
    public class HelperController : BaseController<HelperController>
    {
        public HelperController(
           IMapper mapper, IOptions<AppConfigs> configs):base(mapper, null, configs) { }

        [HttpGet("~/api/[controller]/EncryptString")]
        public JsonResult EncryptString(string str)
        {
            return new JsonResult(EncryptionHelper.Instance.EncryptStringToString_Aes(str));
        }
        [HttpGet("~/api/[controller]/DecryptString")]
        public JsonResult DecryptString(string str)
        {
            var result = EncryptionHelper.Instance.DecryptStringToString_Aes(str);
            return new JsonResult(result);
        }
    }
}

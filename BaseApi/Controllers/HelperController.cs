using AutoMapper;
using Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductCatalogue.AggregateRoute;
using ProductCatalogue.Contacts;
using ProductCatalogue.Contacts.ServiceContracts;
using ProductCatalogue.Models;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ProductCatalogueApi.Controllers
{
    [Route("api/[controller]")]
    public class HelperController : BaseController<HelperController>
    {
        public HelperController(
           IMapper mapper, IOptions<AppConfigs> configs)//, IDataProtectionProvider provider)
            :base(mapper, null, configs) { }

        [HttpGet("~/api/[controller]/EncryptString")]
        public JsonResult EncryptString(string str)
        {
            return new JsonResult(EncryptionHelper.Instance.EncryptString_Aes(str));
            //return new JsonResult(_protector.Protect(str));
        }
        [HttpGet("~/api/[controller]/DecryptString")]
        public JsonResult DecryptString(EncryptionDetails encDetails)
        {
            return new JsonResult(EncryptionHelper.Instance.DecryptString_Aes(encDetails));
            //return new JsonResult(_protector.Unprotect(str));
        }
    }
}

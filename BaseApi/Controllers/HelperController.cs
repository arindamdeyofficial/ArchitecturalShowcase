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
        private readonly IDataProtector _protector;
        private readonly Aes _myAes;
        private readonly byte[] _key;
        private readonly byte[] _four;

        public HelperController(
           IMapper mapper, IOptions<AppConfigs> configs, IDataProtectionProvider provider)
            :base(mapper, null, configs) 
        {
            _protector = provider.CreateProtector(GetType().FullName);
            _myAes = Aes.Create();
            _key = _myAes.Key;// EncryptionHelper.Instance.ConvertStringToByteArrayAscii("nakshal010519871");//_myAes.Key;
            _four = _myAes.IV;
        }

        [HttpGet("~/api/[controller]/EncryptString")]
        public JsonResult EncryptString(string str)
        {
            var result = new EncryptionDetails
            {
                EncryptedText = Convert.ToBase64String(EncryptionHelper.Instance.EncryptString_Aes(str, _key, _four)),
                Key = Convert.ToBase64String(_key),
                Iv = Convert.ToBase64String(_four),
                PlainText = str
            };
            return new JsonResult(result);
            //return new JsonResult(_protector.Protect(str));
        }
        [HttpGet("~/api/[controller]/DecryptString")]
        public JsonResult DecryptString(EncryptionDetails encText)
        {
            var result = new EncryptionDetails
            {
                PlainText = EncryptionHelper.Instance.DecryptString_Aes(Convert.FromBase64String(encText.EncryptedText), Convert.FromBase64String(encText.Key), Convert.FromBase64String(encText.Iv)),
                Key = Convert.ToBase64String(_key),
                Iv = Convert.ToBase64String(_four),
                EncryptedText = encText.EncryptedText
            };

            return new JsonResult(result);
            //return new JsonResult(_protector.Unprotect(str));
        }
    }
    public class EncryptionDetails
    {
        public string EncryptedText { get; set; }
        public string PlainText { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }
    }
}

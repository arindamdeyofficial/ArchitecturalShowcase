using System;

namespace ProductCatalogue.Contacts
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class BaseRequest
    {
        public AppConfigs Configs { get; set; }
    }
}

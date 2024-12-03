using BusinessModel.Interface;
using BusinessModel.Payment.Interface;
using System.Net;

namespace BusinessModel.Common
{
    public class BaseResponse : IBaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}

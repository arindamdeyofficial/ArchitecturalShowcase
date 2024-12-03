using System.Configuration;
using System.Net;
using System.Reflection.Metadata;

namespace BusinessModel.Interface
{
    public interface IBaseResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}

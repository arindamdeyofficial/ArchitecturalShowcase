

using HttpClientConnect;

namespace HttpClientConnect
{
    public interface IHttpClientHelper
    {
        Task InitializeServiceUrlsAsync();
        void SetBaseUrl(HttpServiceEnum serviceEnum);
        void SetDefaultHeaders(string contentType = "application/json", string accept = "application/json");
        Task<T> GetAsync<T>(HttpServiceEnum serviceEnum, string url);
        Task<T> PostAsync<T>(HttpServiceEnum serviceEnum, string url, object data);
        Task<T> PutAsync<T>(HttpServiceEnum serviceEnum, string url, object data);
        Task<T> DeleteAsync<T>(HttpServiceEnum serviceEnum, string url);
    }
}

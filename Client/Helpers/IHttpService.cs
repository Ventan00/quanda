using System.Threading.Tasks;

namespace Quanda.Client.Helpers
{
    public interface IHttpService
    {
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
        Task<HttpResponseWrapper<TR>> PostWithResponse<T, TR>(string url, T data);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T data);
    }
}

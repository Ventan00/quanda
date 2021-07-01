using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quanda.Client.Helpers
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient httpClient;

        private static JsonSerializerOptions DefaultJsonSerializerOptions =>  new() { PropertyNameCaseInsensitive = true };

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(url);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(responseHTTP, DefaultJsonSerializerOptions);
                return new HttpResponseWrapper<T>(response, true, responseHTTP);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, false, responseHTTP);
            }
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {
            var dataJson = JsonSerializer.Serialize(data);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHTTP = await httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        private static async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, options);
        }
    }
}

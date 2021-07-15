using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quanda.Server.Services.Interfaces;

namespace Quanda.Server.Services.Implementations
{
    public class CaptchaService : ICaptchaService
    {
        private const string GoogleApiUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string _secret;

        public CaptchaService(IConfiguration configuration)
        {
            _secret = configuration["CaptchaSecret"];
        }

        private struct GoogleResponse
        {
            public bool Success { get; set; }
        }

        public async Task<bool> VerifyCaptchaAsync(string responseToken)
        {
            using (var httpClient = new HttpClient())
            {
                var dict = new Dictionary<string, string> { { "secret", _secret }, { "response", responseToken } };

                var response = await httpClient.PostAsync(GoogleApiUrl, new FormUrlEncodedContent(dict));
                var stringResponse = await response.Content.ReadAsStringAsync();
                var googleResponse = JsonSerializer.Deserialize<GoogleResponse>(stringResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return googleResponse.Success;
            }
        }
    }
}

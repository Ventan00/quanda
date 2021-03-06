using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Toolbelt.Blazor;

namespace Quanda.Client.Authentication
{
    //Klasa pozwalająca zarządzać zdarzeniami wykonywanymi przed / po wysłaniu rządań przy uzyciu HTTP Client
    public class HttpClientInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly IRefreshTokenService _refreshTokenService;

        public HttpClientInterceptorService(
            HttpClientInterceptor interceptor, IRefreshTokenService refreshTokenService)
        {
            _interceptor = interceptor;
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Dodaje zdarzenia wykonywane przed/po rządaniu http
        /// </summary>
        public void RegisterEvents()
        {
            _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        }

        /// <summary>
        /// Usuwa zarejestrowane zdarzenia wykonywane przed/po rządaniu http
        /// </summary>
        public void DisposeEvents()
        {
            _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
        }

        /// <summary>
        /// Zdarzenie wykonywane przed wysłaniem rzadania HTTP.
        /// Ma na celu wykonanie próby odświezenia tokenów, jeżeli accessToken jest bliski wygaśnięcia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Meta dane HTTP dostarczone przed lub po wykonaniu rządania HTTP</param>
        /// <returns></returns>
        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri?.AbsolutePath;
            if (string.IsNullOrEmpty(absPath))
                return;

            var excludedPaths = new List<string>()
            {
                "/accounts/refresh"
            };

            if (excludedPaths.Any(path => absPath.Contains(path)))
                return;

            var refreshResponse = await _refreshTokenService.TryRefreshTokenAsync();
            if (refreshResponse is not null)
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", refreshResponse.AccessToken);

                if (absPath.Contains("/accounts/logout"))
                {
                    e.Request.Content = new StringContent(
                        JsonSerializer.Serialize(new LogoutDTO { RefreshToken = refreshResponse.RefreshToken }),
                        Encoding.UTF8,
                        "application/json");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace Quanda.Client.Authentication
{
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

        public void RegisterEvents()
        {
            _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        }

        public void DisposeEvents()
        {
            _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
        }

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri?.AbsolutePath;
            if (string.IsNullOrEmpty(absPath))
                return;

            var excludedPaths = new List<string>()
            {
                "/accounts/refresh"
            };

            if (!excludedPaths.Any(path => absPath.Contains(path)))
            {
                var token = await _refreshTokenService.TryRefreshTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
            }
        }
    }
}

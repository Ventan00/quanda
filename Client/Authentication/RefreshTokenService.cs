using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Quanda.Client.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthenticationService _authService;
        private readonly ILocalStorageService _localStorage;

        public RefreshTokenService(
            AuthenticationStateProvider authProvider,
            IAuthenticationService authService,
            ILocalStorageService localStorage)
        {
            _authProvider = authProvider;
            _authService = authService;
            _localStorage = localStorage;
        }

        public async Task<bool> TryRefreshTokenAsync()
        {
            if (!await _localStorage.ContainKeyAsync("access_token") ||
                !await _localStorage.ContainKeyAsync("refresh_token"))
                return false;

            var authState = await _authProvider.GetAuthenticationStateAsync();
            var exp = authState.User.FindFirst(c => c.Type.Equals("exp"))?.Value;

            if (string.IsNullOrEmpty(exp))
                return false;

            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

            var diff = expTime - DateTime.UtcNow;
            if (diff.TotalMinutes <= 2)
                return await _authService.RefreshTokenAsync();

            return false;
        }
    }
}

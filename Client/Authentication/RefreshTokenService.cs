using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Authentication
{
    /// <summary>
    /// Serwis umożliwiający odświeżnie tokenów
    /// </summary>
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

        /// <summary>
        /// Metoda sprawdzająca czy wymagane i możliwe jest odświeżenie tokenów,
        /// jezeli tak wywoływana jest metoda odświeżająca tokeny z AuthenticationService
        /// </summary>
        /// <returns>
        /// Null - jezeli nie jest wymagane odświeżenie tokenów
        /// RefreshResponseDto(AccessToken,RefreshToken) - jezeli tokeny zostają odświeżone
        /// </returns>
        public async Task<RefreshResponseDTO> TryRefreshTokenAsync()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            var exp = authState.User.FindFirst(c => c.Type.Equals("exp"))?.Value;

            if (string.IsNullOrEmpty(exp))
                return null;

            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

            var diff = expTime - DateTime.UtcNow;
            if (diff.TotalMinutes <= 2)
            {
                return await _authService.RefreshTokenAsync();
            }

            return null;
        }
    }
}

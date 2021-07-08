using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(
            IUsersRepository usersRepository,
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _usersRepository = usersRepository;
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var result = await _usersRepository.LoginAsync(loginDto);

            if (result.LoginStatus != LoginStatusEnum.LOGIN_ACCEPTED)
                return null;

            await _localStorage.SetItemAsync("access_token", result.AccessToken);
            await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", result.AccessToken);

            return result;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("access_token");
            await _localStorage.RemoveItemAsync("refresh_token");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
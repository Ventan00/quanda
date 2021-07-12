using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
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
        private readonly IHttpService _httpService;
        private readonly IUsersRepository _usersRepository;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navManager;
        private readonly IToastService _toastService;

        public AuthenticationService(
            IHttpService httpService,
            IUsersRepository usersRepository,
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage,
            NavigationManager navManager,
            IToastService toastService)
        {
            _httpService = httpService;
            _usersRepository = usersRepository;
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _navManager = navManager;
            _toastService = toastService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var result = await _usersRepository.LoginAsync(loginDto);

            if (result.LoginStatus != LoginStatusEnum.LOGIN_ACCEPTED)
                return result;

            await _localStorage.SetItemAsync("access_token", result.AccessToken);
            await _localStorage.SetItemAsync("refresh_token", result.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", result.AccessToken);

            return result;
        }

        public async Task LogoutAsync(bool notifyServer = false)
        {
            if (notifyServer)
            {
                var logoutDto = new LogoutDTO
                {
                    RefreshToken = await _localStorage.GetItemAsync<string>("refresh_token")
                };

                await _httpService.Post("api/accounts/logout", logoutDto);
            }


            await _localStorage.RemoveItemAsync("access_token");
            await _localStorage.RemoveItemAsync("refresh_token");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _navManager.NavigateTo("/login");
        }

        public async Task<RefreshResponseDTO> RefreshTokenAsync()
        {
            var accessToken = await _localStorage.GetItemAsync<string>("access_token");
            var refreshToken = await _localStorage.GetItemAsync<string>("refresh_token");

            var refreshDto = new RefreshRequestDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var response = await _httpService.PostWithResponse<RefreshRequestDTO, RefreshResponseDTO>
                ("api/accounts/refresh", refreshDto);

            if (!response.Success)
            {
                _toastService.ShowError("Your session has expired, please log in.");
                await LogoutAsync();
                return null;
            }

            await _localStorage.SetItemAsync("access_token", response.Response.AccessToken);
            await _localStorage.SetItemAsync("refresh_token", response.Response.RefreshToken);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", response.Response.AccessToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Response.AccessToken);

            return response.Response;
        }
    }
}
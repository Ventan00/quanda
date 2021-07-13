using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Quanda.Client.Authentication
{
    /// <summary>
    /// Klasa pozwalająca aplikacji uzyskać dane dotyczące stanu uwierzytelniania użytkownika, oraz pozwalająca nim zarządzać
    /// </summary>
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        /// <summary>
        /// Metoda pozwalająca uzyskać stan uwierzytelnienia danego użytkownika
        /// </summary>
        /// <returns>
        /// Stan uwierzytelnienia użytkownika wraz z 'user claims'
        /// </returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _localStorageService.GetItemAsync<string>("access_token");
            var refreshToken = await _localStorageService.GetItemAsync<string>("refresh_token");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return _anonymous;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        JwtParser.ParseClaimsFromJwt(accessToken), "jwtAuthType")));
        }

        /// <summary>
        /// Metoda zmieniajaca stan uwierzytelnienia użytkownika na uwierzytelniony (zalogowany), z uprawnieniami zgodnymi z podanym tokenem
        /// </summary>
        /// <param name="token">
        /// jwt (accessToken)
        /// </param>
        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        /// <summary>
        /// Metoda zmieniajaca stan uwierzytelnienia użytkownika na nieuwierzytelniony (niezalogowany)
        /// </summary>
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}

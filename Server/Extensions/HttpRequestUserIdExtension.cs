using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Quanda.Server.Extensions
{
    /// <summary>
    /// Klasa dodająca metody rozszerzeń do klasy HttpRequest
    /// pozwaląjące zarządzać id uwierzytelnionego użytkownika
    /// </summary>
    public static class HttpRequestUserIdExtension
    {
        public static int? IdUser;

        /// <summary>
        /// Metoda pozwalająca dodać id uzytkownika do obiektu HttpRequest
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="claimsPrincipal">Odszyfrowane 'claimsy' z accessTokena</param>
        public static void SetUserId(this HttpRequest httpRequest, ClaimsPrincipal claimsPrincipal)
        {
            var nameIdentifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IdUser = int.Parse(nameIdentifier);
        }

        /// <summary>
        /// Metoda pozwalająca uzyskać id uzytkownika wykonującego dane rządanie
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns>null lub id uzytkownika w zalezności od tego czy wymagane jest uwierzytelnienie</returns>
        public static int? GetUserId(this HttpRequest httpRequest)
        {
            return IdUser;
        }
    }
}

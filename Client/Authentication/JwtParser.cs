using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Quanda.Client.Authentication
{
    /// <summary>
    /// Klasa pomocnicza odpowiedzialna za parsowanie JWT
    /// </summary>
    public class JwtParser
    {
        /// <summary>
        /// Metoda odkodowywujaca JWT oraz wyciagajaca z payload user claims
        /// </summary>
        /// <param name="jwt">Token (jwt)</param>
        /// <returns>
        /// Kolekcja (IEnumerable)<Claim> wyciagnietych z podanego tokenu
        /// </returns>
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();

            var payload = jwt.Split(".")[1];

            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            ExtractRolesFromJWT(claims, keyValuePairs);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        /// <summary>
        /// Pomocnicza metoda wyciagajaca role z podanego slownika, zasilajaca podana liste claimsow
        /// </summary>
        /// <param name="claims">Lista wynikowa, do ktorej zostana dodane wyciagniete role</param>
        /// <param name="keyValuePairs">Slownik (klucz-wartosc) zawierajace dane z payload jwt</param>
        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles is not null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(",");

                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }
    }
}

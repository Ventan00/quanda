using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quanda.Server.Models.Settings;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Implementations
{
    /// <summary>
    /// Pomocniczy serwis do JWT
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtConfigModel _jwtConfigModel;

        public JwtService(IOptionsMonitor<JwtConfigModel> optionsMonitor)
        {
            _jwtConfigModel = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// Metoda parsująca SecruityToken do stringa
        /// </summary>
        /// <param name="securityToken"></param>
        /// <returns>
        /// JWT - w stringu
        /// </returns>
        public string WriteToken(SecurityToken securityToken)
        {
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        /// <summary>
        /// Metoda generująca refreshToken
        /// </summary>
        /// <returns>
        /// Krotka zawierajaca refreshToken oraz jego date wygasniecia
        /// </returns>
        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtConfigModel.RefreshTokenValidityInMinutes);

            return (refreshToken, expirationDate);
        }

        /// <summary>
        /// Metoda generujaca accessToken dla podanego uzytkownika
        /// </summary>
        /// <param name="user">Uzytkownik wraz z dolaczonymi do niego rolami</param>
        /// <returns>JwtSecurityToken</returns>
        public JwtSecurityToken GenerateAccessToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            };

            userClaims.AddRange(
                user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.IdRoleNavigation.Name))
                );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigModel.SecretKey));

            return new JwtSecurityToken(
                issuer: _jwtConfigModel.Issuer,
                audience: _jwtConfigModel.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigModel.AccessTokenValidityInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        }

        /// <summary>
        /// Metoda generujaca token sluzacy do odzyskania hasla dla podanego uzytkownika
        /// </summary>
        /// <param name="user">Uzytkownik</param>
        /// <returns>JwtSecruityToken</returns>
        public JwtSecurityToken GeneratePasswordRecoveryToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.HashedPassword + "-" + user.RegistrationDate));

            return new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfigModel.PasswordRecoveryTokenValidityInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        }

        /// <summary>
        /// Metoda wyciagajaca ClaimsPrinicpal z podanego JWT wydanego do odzyskania hasla
        /// </summary>
        /// <param name="jwt">JWT wydany do odzyskania hasla</param>
        /// <param name="user">User dla którego wydany został token</param>
        /// <returns>
        /// Null lub ClaimsPrinipal danego tokena w zaleznosci od rezultatu metody 'ValidateAndGetPrincipalFromJwt'
        /// </returns>
        public ClaimsPrincipal GetPrincipalFromPasswordRecoveryToken(string jwt, User user)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.HashedPassword + "-" + user.RegistrationDate)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return ValidateAndGetPrincipalFromJwt(jwt, tokenValidationParameters);
        }

        /// <summary>
        /// Metoda wyciagajaca ClaimsPrinicpal z podanego JWT wydanego jako accessToken
        /// </summary>
        /// <param name="jwt">JWT wydany jako accessToken</param>
        /// <returns>
        /// Null lub ClaimsPrinipal danego tokena w zaleznosci od rezultatu metody 'ValidateAndGetPrincipalFromJwt'
        /// </returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string jwt)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfigModel.Issuer,
                ValidAudience = _jwtConfigModel.Audience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtConfigModel.SecretKey))
            };

            return ValidateAndGetPrincipalFromJwt(jwt, tokenValidationParameters);
        }

        /// <summary>
        /// Metoda pomocnicza walidująca oraz deszyfrujaca JWT wedlug podanych parametrow
        /// </summary>
        /// <param name="jwt">Token do walidacji oraz deszyfrowania</param>
        /// <param name="tokenValidationParameters">Parametry wedlug ktorych ma byc walidowany token</param>
        /// <returns>
        /// ClaimsPrincipal - wyciagniete z podanego JWT, lub
        /// Null - w przypadku gdy walidacja przebiegnie niepomyslnie
        /// </returns>
        private ClaimsPrincipal ValidateAndGetPrincipalFromJwt(string jwt, TokenValidationParameters tokenValidationParameters)
        {
            try
            {
                ClaimsPrincipal principal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwt, tokenValidationParameters, out var securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

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
    public class JwtService : IJwtService
    {
        private readonly JwtConfigModel _jwtConfigModel;

        public JwtService(IOptionsMonitor<JwtConfigModel> optionsMonitor)
        {
            _jwtConfigModel = optionsMonitor.CurrentValue;
        }

        public string WriteToken(SecurityToken securityToken)
        {
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            var expirationDate = DateTime.Now.AddMinutes(_jwtConfigModel.RefreshTokenValidityInMinutes);

            return (refreshToken, expirationDate);
        }

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
                expires: DateTime.Now.AddMinutes(_jwtConfigModel.AccessTokenValidityInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        }

        public JwtSecurityToken GeneratePasswordRecoveryToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.HashedPassword + "-" + user.RegistrationDate));

            return new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(_jwtConfigModel.PasswordRecoveryTokenValidityInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        }

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

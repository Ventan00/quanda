using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfigurationSection _jwtConfigurationSection;

        public JwtService(IConfiguration configuration)
        {
            _jwtConfigurationSection = configuration.GetSection("JwtSettings");
        }

        public string WriteToken(SecurityToken securityToken)
        {
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            var expirationDate = DateTime.Now.AddMinutes(int.Parse(_jwtConfigurationSection["RefreshTokenValidityInMinutes"]));

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurationSection["SecretKey"]));

            return new JwtSecurityToken(
                issuer: _jwtConfigurationSection["Issuer"],
                audience: _jwtConfigurationSection["Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(int.Parse(_jwtConfigurationSection["AccessTokenValidityInMinutes"])),
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
                expires: DateTime.Now.AddMinutes(int.Parse(_jwtConfigurationSection["PasswordRecoveryTokenValidityInMinutes"])),
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
                ValidIssuer = _jwtConfigurationSection["Issuer"],
                ValidAudience = _jwtConfigurationSection["Audience"],
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtConfigurationSection["SecurityKey"]))
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

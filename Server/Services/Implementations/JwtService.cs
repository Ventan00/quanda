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

        public void AddTokensToCookies(string refreshToken, DateTime refreshTokenExpirationDate, JwtSecurityToken accessToken, IResponseCookies responseCookies)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(10),
            };

            responseCookies.Append("access_token", new JwtSecurityTokenHandler().WriteToken(accessToken), cookieOptions);
            responseCookies.Append("refresh_token", refreshToken, cookieOptions);
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
    }
}

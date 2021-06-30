using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Interfaces
{
    public interface IJwtService
    {
        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken();
        public JwtSecurityToken GenerateAccessToken(User user);

        public void AddTokensToCookies(string refreshToken, DateTime refreshTokenExpirationDate,
            JwtSecurityToken accessToken, IResponseCookies responseCookies);
    }
}

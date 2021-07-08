using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Interfaces
{
    public interface IJwtService
    {
        public string WriteToken(SecurityToken securityToken);
        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken();
        public JwtSecurityToken GenerateAccessToken(User user);
        public JwtSecurityToken GeneratePasswordRecoveryToken(User user);
        public ClaimsPrincipal GetPrincipalFromPasswordRecoveryToken(string jwt, User user);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string jwt);
    }
}

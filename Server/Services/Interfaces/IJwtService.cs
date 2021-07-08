using System;
using System.IdentityModel.Tokens.Jwt;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Interfaces
{
    public interface IJwtService
    {
        public (string refreshToken, DateTime expirationDate) GenerateRefreshToken();
        public JwtSecurityToken GenerateAccessToken(User user);
        public JwtSecurityToken GeneratePasswordRecoveryToken(User user);
        public int? DecryptPasswordRecoveryToken(string jwt, User user);
    }
}

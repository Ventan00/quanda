using Microsoft.AspNetCore.Identity;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Implementations
{
    public class UserAuthService : IUserAuthService
    {
        public bool VerifyUserPassword(string rawPassword, User user)
        {
            var verificationRs = new PasswordHasher<User>().VerifyHashedPassword(null, user.HashedPassword, rawPassword);
            return @verificationRs switch
            {
                PasswordVerificationResult.Success => true,
                _ => false
            };
        }
    }
}

using System;
using System.Threading.Tasks;
using Quanda.Server.Enums;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<UserStatus> AddNewUserAsync(RegisterDTO registerDto);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<UserStatus> UpdateRefreshTokenForUserAsync(User user, string refreshToken, DateTime refreshTokenExpirationDate);
    }
}

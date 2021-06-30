using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Utils;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Models;
using static Quanda.Server.Utils.UserStatus;

namespace Quanda.Server.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserStatus> AddNewUserAsync(RegisterDTO registerDto)
        {
            var isEmailTaken = await _context.Users.AnyAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());
            if (isEmailTaken)
                return USER_EMAIL_IS_TAKEN;

            var isNicknameTaken = await _context.Users.AnyAsync(u => u.Nickname.ToLower() == registerDto.Nickname.ToLower());
            if (isNicknameTaken)
                return USER_NICKNAME_IS_TAKEN;

            var user = new User
            {
                Nickname = registerDto.Nickname,
                Email = registerDto.Email
            };

            user.HashedPassword = new PasswordHasher<User>().HashPassword(user, registerDto.RawPassword);

            await _context.Users.AddAsync(user);

            var isRegistered = await _context.SaveChangesAsync() > 0;
            return !isRegistered ? USER_DB_ERROR : USER_REGISTERED;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserStatus> UpdateRefreshTokenForUserAsync(User user, string refreshToken, DateTime refreshTokenExpirationDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationDate = refreshTokenExpirationDate;

            return await _context.SaveChangesAsync() > 0 ? USER_REFRESH_TOKEN_UPDATED : USER_DB_ERROR;
        }
    }
}

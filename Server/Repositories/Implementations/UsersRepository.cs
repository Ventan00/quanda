using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Models;
using static Quanda.Server.Utils.UserStatus;

namespace Quanda.Server.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserStatus> AddNewUserAsync(RegisterDTO registerDto, string confirmationCode)
        {
            var isEmailTaken = await _context.Users.AnyAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());
            if (isEmailTaken)
                return USER_EMAIL_IS_TAKEN;

            var isNicknameTaken =
                await _context.Users.AnyAsync(u => u.Nickname.ToLower() == registerDto.Nickname.ToLower());
            if (isNicknameTaken)
                return USER_NICKNAME_IS_TAKEN;

            var user = new User
            {
                Nickname = registerDto.Nickname,
                Email = registerDto.Email,
                RegistrationDate = DateTime.Now
            };

            user.HashedPassword = new PasswordHasher<User>().HashPassword(user, registerDto.RawPassword);

            await _context.Users.AddAsync(user);

            var tempUser = new TempUser
            {
                Code = confirmationCode,
                ExpirationDate =
                    DateTime.Now.AddMinutes(int.Parse(_configuration["RegisterEmailConfirmationMaxTimeInMinutes"])),
                IdUserNavigation = user
            };

            await _context.TempUsers.AddAsync(tempUser);

            var isRegistered = await _context.SaveChangesAsync() > 0;
            return !isRegistered ? USER_DB_ERROR : USER_REGISTERED;
        }

        public async Task<User> GetUserByNicknameOrEmailAsync(string nicknameOrEmail)
        {
            var foundUser = await GetUserWithDetailsByAsync(u => u.Email == nicknameOrEmail);
            if (foundUser is null)
                foundUser = await GetUserWithDetailsByAsync(u => u.Nickname == nicknameOrEmail);

            return foundUser;
        }

        public async Task<UserStatus> UpdateRefreshTokenForUserAsync(User user, string refreshToken,
            DateTime? refreshTokenExpirationDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationDate = refreshTokenExpirationDate;

            return await _context.SaveChangesAsync() > 0 ? USER_REFRESH_TOKEN_UPDATED : USER_DB_ERROR;
        }

        public async Task<User> GetUserByIdAsync(int idUser)
        {
            return await GetUserWithDetailsByAsync(u => u.IdUser == idUser);
        }

        public async Task<bool> SetNewPasswordForUser(User user, string rawPassword)
        {
            user.HashedPassword = new PasswordHasher<User>().HashPassword(user, rawPassword);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await GetUserWithDetailsByAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await GetUserWithDetailsByAsync(u => u.Email == email);
        }

        public async Task<UserProfileDetailsResponseDto> GetUserProfileDetailsAsync(int idUser)
        {
            var details = await _context.Users
                .Where(u => u.IdUser == idUser)
                .Select(u => new UserProfileDetailsResponseDto
                {
                    Nickname = u.Nickname,
                    Avatar = u.Avatar,
                    Bio = u.Bio,
                    IsPremium = u.UserRoles.Any(ur => ur.IdRoleNavigation.Name == "Premium" &&
                                                      ur.ExpirationDate > DateTime.UtcNow),
                    Points = u.TagUsers.Sum(tu => tu.Points),
                    Questions = u.Questions.Count,
                    Answers = u.Answers.Count,
                    ProfileViews = u.Views,
                    RegisteredAt = u.RegistrationDate,
                    TopTags = u.TagUsers
                        .OrderByDescending(tu => tu.Points)
                        .Select(tu => new UserInTagResponseDto
                        {
                            IdTag = tu.IdTagNavigation.IdTag,
                            TagName = tu.IdTagNavigation.Name,
                            UserPoints = tu.Points
                        })
                }).FirstOrDefaultAsync();

            var top3Users = await _context.TagUsers
                .GroupBy(tag => tag.IdUser)
                .OrderByDescending(tag => tag.Sum(t => t.Points))
                .Take(3)
                .Select(gr => gr.Key)
                .ToListAsync();
            var index = top3Users.IndexOf(idUser);
            details.TopUsersPlacement = index != -1 ? index : null;

            return details;
        }

        public async Task AddViewForUserAsync(int idUser)
        {
            User templateUser;
            _context.Users.FromSqlInterpolated(
                $"UPDATE Users SET {nameof(templateUser.Views)} = {nameof(templateUser.Views)} + 1");

            await _context.SaveChangesAsync();
        }

        private async Task<User> GetUserWithDetailsByAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.Include(u => u.IdTempUserNavigation).Include(u => u.UserRoles)
                .ThenInclude(ur => ur.IdRoleNavigation).SingleOrDefaultAsync(predicate);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<UserStatus> AddNewUserAsync(RegisterDTO registerDto, string confirmationCode);
        public Task<User> GetUserByNicknameOrEmailAsync(string nicknameOrEmail);

        public Task<UserStatus> UpdateRefreshTokenForUserAsync(User user, string refreshToken,
            DateTime? refreshTokenExpirationDate);

        public Task<User> GetUserByIdAsync(int idUser);
        public Task<bool> SetNewPasswordForUser(User user, string rawPassword);
        public Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<UserProfileDetailsResponseDto> GetUserProfileDetailsAsync(int idUser);
        public Task AddViewForUserAsync(int idUser);
      
        /// <summary>
        ///     Funkcja zwracająca top 3 użytkowników
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Top3UserResponseDTO>> GetTop3UsersAsync();
    }
}
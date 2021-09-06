using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        /// <summary>
        ///     Wysłanie do api rządania o zalogowanie
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);

        /// <summary>
        ///     Wysłanie do api rządania o rejestracje
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto);

        /// <summary>
        ///     Wysłanie do api rządania o odzyskanie emailu potwierdzającego nowo-zarejestrowane konto
        /// </summary>
        /// <param name="recoverDto"></param>
        /// <returns></returns>
        public Task<bool> RecoverConfirmationEmailAsync(RecoverDTO recoverDto);

        /// <summary>
        ///     Wysłanie do api rządania o odzyskanie hasła
        /// </summary>
        /// <param name="recoverDto"></param>
        /// <returns></returns>
        public Task<bool> RecoverPasswordAsync(RecoverDTO recoverDto);

        /// <summary>
        ///     Wysłanie do api rządania o ustawienie nowego hasła
        /// </summary>
        /// <param name="passwordResetDto"></param>
        /// <returns></returns>
        public Task<HttpStatusCode> ResetPasswordAsync(PasswordResetDTO passwordResetDto);

        /// <summary>
        ///     Funkcja która zwraca listę 3 użytkowników z największą ilością punktów
        /// </summary>
        /// <returns></returns>
        public Task<List<Top3UserResponseDTO>> GetTop3Users();
    }
}
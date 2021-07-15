using System.Net;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
        public Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto);
        public Task<bool> RecoverConfirmationEmailAsync(RecoverDTO recoverDto);
        public Task<bool> RecoverPasswordAsync(RecoverDTO recoverDto);
        public Task<HttpStatusCode> ResetPasswordAsync(PasswordResetDTO passwordResetDto);
    }
}

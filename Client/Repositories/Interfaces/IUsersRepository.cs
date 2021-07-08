using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<LoginStatusEnum> LoginAsync(LoginDTO loginDto);
        public Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto);
        public Task RecoverConfirmationEmailAsync(RecoverDTO recoverDto);
        public Task RecoverPasswordAsync(RecoverDTO recoverDto);
        public Task<bool> ResetPasswordAsync(PasswordResetDTO passwordResetDto);
        public Task<GetUserForQuestionByIDDTO> GetDataForQuestionAsync(int questionIdUser);
    }
}

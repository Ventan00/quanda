using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<LoginStatusEnum> LoginAsync(LoginDTO loginDto);
        public Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto);
        public Task ResendConfirmationEmailAsync(RecoverDTO recoverDto);
    }
}

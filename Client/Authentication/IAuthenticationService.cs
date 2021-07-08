using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Authentication
{
    public interface IAuthenticationService
    {
        public Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
        public Task LogoutAsync();
        public Task<string> RefreshTokenAsync();
    }
}

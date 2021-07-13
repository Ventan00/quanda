using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Authentication
{
    public interface IRefreshTokenService
    {
        public Task<RefreshResponseDTO> TryRefreshTokenAsync();
    }
}

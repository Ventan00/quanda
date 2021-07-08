using System.Threading.Tasks;

namespace Quanda.Client.Authentication
{
    public interface IRefreshTokenService
    {
        public Task<bool> TryRefreshTokenAsync();
    }
}

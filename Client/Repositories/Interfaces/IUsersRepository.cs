using System.Net.Http;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<LoginStatusEnum> LoginAsync(LoginDTO loginDto);
    }
}

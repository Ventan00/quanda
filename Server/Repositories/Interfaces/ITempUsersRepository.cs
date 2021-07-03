using System.Threading.Tasks;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface ITempUsersRepository
    {
        public Task<TempUserResult> DeleteTempUserByCodeAsync(string code);
        public Task<string> GetConfirmationCodeForUserAsync(string email);
        public Task<bool> ExtendValidityAsync(string email);
    }
}

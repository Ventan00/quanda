using System.Threading.Tasks;
using Quanda.Server.Utils;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface ITempUsersRepository
    {
        public Task<TempUserResult> DeleteTempUserByCodeAsync(string code);
    }
}

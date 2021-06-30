using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;

namespace Quanda.Server.Repositories.Implementations
{
    public class TempUsersRepository : ITempUsersRepository
    {
        private readonly AppDbContext _context;
        public TempUsersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TempUserResult> DeleteTempUserByCodeAsync(string code)
        {
            var tempUser = await _context.TempUsers.SingleOrDefaultAsync(tu => tu.Code == code);
            if (tempUser == null)
                return TempUserResult.TEMP_USER_NOT_FOUND;

            _context.TempUsers.Remove(tempUser);

            var isDeleted = await _context.SaveChangesAsync() > 0;
            return isDeleted ? TempUserResult.TEMP_USER_DELETED : TempUserResult.TEMP_USER_DB_ERROR;
        }
    }
}

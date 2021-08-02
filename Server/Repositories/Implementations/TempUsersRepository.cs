using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Implementations
{
    public class TempUsersRepository : ITempUsersRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public TempUsersRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public async Task<string> GetConfirmationCodeForUserAsync(string email)
        {
            var tempUser = await GetTempUserByEmailAsync(email);

            return tempUser?.Code;
        }

        public async Task<bool> ExtendValidityAsync(string email)
        {
            var tempUser = await GetTempUserByEmailAsync(email);

            tempUser.ExpirationDate = DateTime.Now.AddMinutes(int.Parse(_configuration["RegisterEmailConfirmationMaxTimeInMinutes"]));

            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<TempUser> GetTempUserByEmailAsync(string email)
        {
            return await _context.TempUsers
                .Include(tu => tu.IdUserNavigation)
                .SingleOrDefaultAsync(tu => tu.IdUserNavigation.Email == email);
        }
    }
}

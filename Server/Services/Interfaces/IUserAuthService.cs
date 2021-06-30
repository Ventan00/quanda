using System;
using System.Threading.Tasks;
using Quanda.Shared.Models;

namespace Quanda.Server.Services.Interfaces
{
    public interface IUserAuthService
    {
        public bool VerifyUserPassword(string rawPassword, User user);
    }
}

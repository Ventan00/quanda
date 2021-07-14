using System.Threading.Tasks;

namespace Quanda.Server.Services.Interfaces
{
    public interface ISmtpService
    {
        public Task SendRegisterConfirmationEmailAsync(string email, string confirmationCode);
        public Task SendPasswordRecoveryEmailAsync(string email, string recoveryJwt, int idUser);
    }
}

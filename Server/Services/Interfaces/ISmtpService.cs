using System.Threading.Tasks;

namespace Quanda.Server.Services.Interfaces
{
    public interface ISmtpService
    {
        public Task SendRegisterConfirmationEmailAsync(string registerDtoEmail, string confirmationCode);
    }
}

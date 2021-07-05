using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quanda.Server.Services.Interfaces;

namespace Quanda.Server.Services.Implementations
{
    public class SmtpService : ISmtpService
    {
        private readonly IConfigurationSection _smtpConfigSection;
        public SmtpService(IConfiguration configuration)
        {
            _smtpConfigSection = configuration.GetSection("SmtpSettings");
        }


        public async Task SendRegisterConfirmationEmailAsync(string email, string confirmationCode)
        {
            var smtpClient = CreateSmtpClient();
            await smtpClient.SendMailAsync(_smtpConfigSection["Email"], email, "QUANDA - Complete register", $"https://localhost:44358/api/accounts/confirm-email/{confirmationCode}");
        }

        public async Task SendPasswordRecoveryEmailAsync(string email, string recoveryJwt, int idUser)
        {
            var smtpClient = CreateSmtpClient();
            await smtpClient.SendMailAsync(_smtpConfigSection["Email"], email, "QUANDA - Recover password", $"https://localhost:44358/recover/password/reset?uuid={idUser}&recovery_token={recoveryJwt}");
        }

        private SmtpClient CreateSmtpClient()
        {
            return new(_smtpConfigSection["Host"])
            {
                Port = int.Parse(_smtpConfigSection["Port"]),
                Credentials = new NetworkCredential(_smtpConfigSection["Email"], _smtpConfigSection["Password"]),
                EnableSsl = bool.Parse(_smtpConfigSection["EnableSsl"]),
            };
        }
    }
}

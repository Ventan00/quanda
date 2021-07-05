using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Quanda.Server.Services.Interfaces;

namespace Quanda.Server.Services.Implementations
{
    public class SmtpService : ISmtpService
    {
        private readonly IConfigurationSection _smtpConfigSection;
        private readonly string _hostBaseUrl;

        public SmtpService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _smtpConfigSection = configuration.GetSection("SmtpSettings");

            if (httpContextAccessor.HttpContext is not null)
            {
                _hostBaseUrl = $"https://{httpContextAccessor.HttpContext.Request.Host}";
            }
        }


        public async Task SendRegisterConfirmationEmailAsync(string email, string confirmationCode)
        {
            var href = $"{_hostBaseUrl}/api/accounts/confirm-email/{confirmationCode}";
            await SendEmailAsync(email, "QUANDA - Complete register", $"{href}");
        }

        public async Task SendPasswordRecoveryEmailAsync(string email, string recoveryJwt, int idUser)
        {
            var href = $"{_hostBaseUrl}/recover/password/reset?uuid={idUser}&recovery_token={recoveryJwt}";
            await SendEmailAsync(email, "QUANDA - Recover password", $"{href}");
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

        private async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var smtpClient = CreateSmtpClient();
            await smtpClient.SendMailAsync(_smtpConfigSection["Email"], recipientEmail, subject, $"{body}");
        }
    }
}

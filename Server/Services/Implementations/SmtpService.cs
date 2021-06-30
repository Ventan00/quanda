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


        public async Task SendRegisterConfirmationEmailAsync(string registerDtoEmail, string confirmationCode)
        {
            var smtpClient = new SmtpClient(_smtpConfigSection["Host"])
            {
                Port = int.Parse(_smtpConfigSection["Port"]),
                Credentials = new NetworkCredential(_smtpConfigSection["Email"], _smtpConfigSection["Password"]),
                EnableSsl = bool.Parse(_smtpConfigSection["EnableSsl"]),
            };

            await smtpClient.SendMailAsync(_smtpConfigSection["Email"], registerDtoEmail, "QUANDA - Complete register", $"https://localhost:44358/api/accounts/confirm-email/{confirmationCode}");
        }
    }
}

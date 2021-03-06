using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Quanda.Server.Models.Settings;
using Quanda.Server.Services.Interfaces;
using Quanda.Server.Utils;

namespace Quanda.Server.Services.Implementations
{
    public class SmtpService : ISmtpService
    {
        private readonly SmtpConfigModel _smpConfigModel;
        private readonly string _hostBaseUrl;

        public SmtpService(IOptionsMonitor<SmtpConfigModel> optionsMonitor, IHttpContextAccessor httpContextAccessor)
        {
            _smpConfigModel = optionsMonitor.CurrentValue;

            if (httpContextAccessor.HttpContext is not null)
            {
                _hostBaseUrl = $"https://{httpContextAccessor.HttpContext.Request.Host}";
            }
        }

        public async Task SendRegisterConfirmationEmailAsync(string email, string confirmationCode)
        {
            var subject = "QUANDA - Complete register";

            var href = $"{_hostBaseUrl}/api/accounts/confirm-email/{confirmationCode}";
            var body = GlobalVariables.AccountConfirmationEmailBodyHtml.Replace("href_template", href);

            await SendEmailAsync(subject, body, true, email);
        }

        public async Task SendPasswordRecoveryEmailAsync(string email, string recoveryJwt, int idUser)
        {
            var subject = "QUANDA - Recover password";

            var href = $"{_hostBaseUrl}/recover/password/reset?uuid={idUser}&recovery_token={recoveryJwt}";
            var body = GlobalVariables.PasswordRecoveryEmailBodyHtml.Replace("href_template", href);

            await SendEmailAsync(subject, body, true, email);
        }

        private SmtpClient CreateSmtpClient()
        {
            return new(_smpConfigModel.Host)
            {
                Port = _smpConfigModel.Port,
                Credentials = new NetworkCredential(_smpConfigModel.Email, _smpConfigModel.Password),
                EnableSsl = _smpConfigModel.EnableSsl,
            };
        }

        private async Task SendEmailAsync(string subject, string body, bool isBodyHtml, params string[] recipients)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(_smpConfigModel.Email);

                foreach (var recipient in recipients)
                {
                    mail.To.Add(recipient);
                }

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isBodyHtml;

                using (var smtpClient = CreateSmtpClient())
                {
                    await smtpClient.SendMailAsync(mail);
                }
            }
        }
    }
}

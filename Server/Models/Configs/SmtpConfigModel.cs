namespace Quanda.Server.Models.Settings
{
    public class SmtpConfigModel
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool EnableSsl { get; set; }
    }
}

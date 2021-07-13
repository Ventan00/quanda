using System.IO;

namespace Quanda.Server.Utils
{
    public static class GlobalVariables
    {
        public static readonly string AccountConfirmationEmailBodyHtml;
        public static readonly string PasswordRecoveryEmailBodyHtml;

        static GlobalVariables()
        {
            AccountConfirmationEmailBodyHtml = File.ReadAllText("Utils/htdocs/AccountConfirmationEmailBody.html");
            PasswordRecoveryEmailBodyHtml = File.ReadAllText("Utils/htdocs/PasswordRecoveryEmailBody.html");
        }
    }
}

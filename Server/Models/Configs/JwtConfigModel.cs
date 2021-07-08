namespace Quanda.Server.Models.Settings
{
    public class JwtConfigModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int RefreshTokenValidityInMinutes { get; set; }
        public int AccessTokenValidityInMinutes { get; set; }
        public int PasswordRecoveryTokenValidityInMinutes { get; set; }
    }
}

using Quanda.Shared.Enums;

namespace Quanda.Shared.DTOs.Responses
{
    public class LoginResponseDTO
    {
        public LoginStatusEnum LoginStatus { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Avatar { get; set; }
    }
}

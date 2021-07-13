using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class LogoutDTO
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}

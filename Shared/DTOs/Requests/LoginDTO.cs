using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(6)]
        public string RawPassword { get; set; }
    }
}

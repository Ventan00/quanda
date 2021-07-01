using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(30)]
        [MinLength(6)]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(6)]
        public string RawPassword { get; set; }
    }
}

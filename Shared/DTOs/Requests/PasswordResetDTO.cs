using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class PasswordResetDTO
    {
        [Required]
        [MaxLength(30)]
        [MinLength(6)]
        public string RawPassword { get; set; }

        [Required]
        public int? IdUser { get; set; }

        [Required]
        public string UrlEncodedRecoveryJwt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class RecoverDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

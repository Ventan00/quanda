using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class UpdateTagDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int? IdMainTag { get; set; }
    }
}

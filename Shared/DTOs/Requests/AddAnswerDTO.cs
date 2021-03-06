using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class AddAnswerDTO
    {
        public string Text { get; set; }

        [Required(ErrorMessage = "IdQuestion is required")]
        public int IdQuestion { get; set; }
        public int? IdRootAnswer { get; set; }
    }
}

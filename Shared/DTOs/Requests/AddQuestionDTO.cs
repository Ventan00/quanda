using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Requests
{
    public class AddQuestionDTO
    {
        [MaxLength(50)]
        [Required]
        public string Header { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int IdUser { get; set; }
    }
}
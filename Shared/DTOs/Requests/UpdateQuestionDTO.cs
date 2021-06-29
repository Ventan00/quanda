using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Requests
{
    public class UpdateQuestionDTO
    {
        [Required]
        public string Description { get; set; }

    }
}

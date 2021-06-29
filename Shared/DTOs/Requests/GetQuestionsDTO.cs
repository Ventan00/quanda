using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Shared.DTOs.Requests
{
    public class GetQuestionsDTO
    {
        public List<Category>? Categories { get; set; }
        [Required]
        public SORT_OPTION_ENUM SortingOption { get; set; }
        public int Skip { get; set; }
    }
}

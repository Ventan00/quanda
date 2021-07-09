﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.DTOs.Requests
{
    public class AddCategoryDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int? IdMainCategory { get; set; }
    }
}

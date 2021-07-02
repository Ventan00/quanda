﻿using System.ComponentModel.DataAnnotations;

namespace Quanda.Shared.DTOs.Requests
{
    public class ResendConfirmationEmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

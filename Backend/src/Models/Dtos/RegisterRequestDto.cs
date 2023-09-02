using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using src.Services;

namespace src.Models.Dtos
{
    public class RegisterRequestDto
    {
        [MinLength(3), MaxLength(255)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [PasswordValidation(ErrorMessage = "Invalid password")]
        public string? Password { get; set; }
    }
}
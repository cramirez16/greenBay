using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models.Dtos
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(200)]
        public required string Name { get; set; }

        [Required, MinLength(6), MaxLength(64)]
        public required string Password { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        public required string Role { get; set; }
    }
}
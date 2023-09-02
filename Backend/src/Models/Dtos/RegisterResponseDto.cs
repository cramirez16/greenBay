using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using src.Services;

namespace src.Models.Dtos
{
    public class RegisterResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        public required DateTime CreationDate { get; set; }
    }
}
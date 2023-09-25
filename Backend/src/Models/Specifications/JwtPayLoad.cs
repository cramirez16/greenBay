using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Src.Models
{
    public class JwtPayLoad
    {
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required string Money { get; set; }
    }
}
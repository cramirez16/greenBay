using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Src.Models;
using Src.Models.Dtos;

namespace Src.Services.IServices
{
    public interface IJWTService
    {
        public string CreateToken(JwtPayLoad jwtPayLoad);
        public UserResponseDto? GetClaimsFromToken(string token);
    }
}



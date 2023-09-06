using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using src.Models;
using src.Models.Dtos;

namespace src.Services.IServices
{
    public interface IJWTService
    {
        public string CreateToken(JwtPayLoad jwtPayLoad);
        public UserResponseDto? GetClaimsFromToken(string token);
    }
}



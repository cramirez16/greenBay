using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models.Dtos;
using Backend.Services;
using src.Models;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GreenBayDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly JWTHandler _tokenHandler;

        public UserController(GreenBayDbContext context, JWTHandler tokenHandler, ILogger<UserController> logger)
        {
            _context = context;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var userByEmail = await _context.TblUsers.FirstOrDefaultAsync(
                user => user.Email == loginDto.Email
            );

            if (userByEmail == null)
            {
                _logger.LogInformation("Login rejected, wrong Email.");
                return Unauthorized(new { wrongEmail = true });
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, userByEmail.Password))
            {
                _logger.LogInformation("Login rejected, wrong Password.");
                return Unauthorized(new { wrongPassword = true });
            }

            var jwtPayLoad = new JwtPayLoad
            {
                UserId = userByEmail.Id.ToString(),
                Name = userByEmail.Name,
                Role = userByEmail.Role,
                Email = userByEmail.Email,
                Money = userByEmail.Money.ToString(),
            };

            _logger.LogInformation("Login was successfully.");
            return Ok(new { tokenKey = _tokenHandler.CreateToken(jwtPayLoad) });
        }

    }
}
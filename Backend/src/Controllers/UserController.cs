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
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly GreenBayDbContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly JWTHandler _tokenHandler;
        private readonly IMapper _automapper;

        public UserController(
            GreenBayDbContext context,
            JWTHandler tokenHandler,
            ILogger<UserController> logger,
            IMapper automapper
        )
        {
            _context = context;
            _tokenHandler = tokenHandler;
            _logger = logger;
            _automapper = automapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            // Given any parameters missing, the user can't sign in and the application
            // displays a message listing the missing parameters

            if (loginDto.Email == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (email) in the request.");
                return BadRequest(new { missingEmail = true });
            }

            if (loginDto.Password == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (password) in the request.");
                return BadRequest(new { missingPassword = true });
            }

            // Read from the database the user data with the email = loginDto.Email 
            User? userByEmail = await _context.TblUsers.FirstOrDefaultAsync(
                user => user.Email == loginDto.Email
            );

            // Given the username is not existing, 
            // the user can't sign in and the application displays a message about it
            if (userByEmail == null)
            {
                _logger.LogInformation("Login rejected, wrong Email.");
                return Unauthorized(new { wrongEmail = true });
            }

            // Given the password is not matching the one stored with the username,
            // the user can't sign in and the application displays message that the password is wrong

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, userByEmail.Password))
            {
                _logger.LogInformation("Login rejected, wrong Password.");
                return Unauthorized(new { wrongPassword = true });
            }

            // Given the username and password pair is correct, a token is returned to initiate further
            // requests and the current amount of greenBay dollars the user has
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

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (request.Name == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (name) in the request.");
                return BadRequest(new { missingName = true });
            }

            if (request.Email == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (email) in the request.");
                return BadRequest(new { missingEmail = true });
            }

            if (request.Password == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (password) in the request.");
                return BadRequest(new { missingPassword = true });
            }

            var userByEmail = await _context.TblUsers.FirstOrDefaultAsync(
                user => user.Email == request.Email
            );
            if (userByEmail != null)
            {
                _logger.LogInformation("Register rejected, email already exists.");
                return Conflict(new { takenEmail = true });
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var createdUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                Role = "User",
                Money = 100.00m,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            await _context.TblUsers.AddAsync(createdUser);
            await _context.SaveChangesAsync();

            var createdUserDto = new RegisterResponseDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                Name = createdUser.Name,
                CreationDate = createdUser.CreationDate
            };
            _logger.LogInformation("New user registered.");
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUserDto);
        }

        [HttpGet()] // localhost/api/User
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            var users = await _context.TblUsers.ToListAsync();
            _logger.LogInformation("User list sent.");
            return Ok(_automapper.Map<List<UserResponseDto>>(users));
        }

        [HttpGet("{id}")] // localhost/api/User/{id}
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            // id never is null, if id is not in the url, the request never arrive to this endpoint.
            // get localhost/api/User/{id} , no id => get localhost/api/User
            var authenticatedUser = HttpContext.User;
            var userIdClaim = authenticatedUser.Claims.FirstOrDefault(
                claim => claim.Type == "userId"
            );

            User? userById = await _context.TblUsers.FindAsync(id);
            if (userById == null)
            {
                _logger.LogInformation("User not found.");
                return NotFound(new { wrongId = true });
            }
            if (userIdClaim == null)
            {
                _logger.LogInformation("Access to user data forbidden.");
                return Forbid();
            }
            if (
                (userById.Id.ToString() != userIdClaim.Value)
                && !authenticatedUser.IsInRole("Admin")
            )
            {
                _logger.LogInformation("Access to user data forbidden.");
                return Forbid();
            }
            _logger.LogInformation("User data sent.");
            return Ok(_automapper.Map<UserResponseDto>(userById));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateDto user)
        {
            var authenticatedUser = HttpContext.User;
            var userIdClaim = authenticatedUser.Claims.FirstOrDefault(
                claim => claim.Type == "userId"
            );

            var userById = await _context.TblUsers.FindAsync(id);

            if (userById == null)
            {
                _logger.LogInformation("User data not found, wrong id.");
                return NotFound(new { wrongId = true });
            }

            if (userIdClaim == null)
            {
                _logger.LogInformation("Update user data forbidden.");
                return Forbid();
            }

            if (
                (userById.Id.ToString() != userIdClaim.Value)
                && !authenticatedUser.IsInRole("Admin")
            )
            {
                _logger.LogInformation("Update user data forbidden.");
                return Forbid();
            }

            var userByEmail = await _context.TblUsers.FirstOrDefaultAsync(
                userFound => userFound.Email == user.Email
            );
            if (userByEmail != null && userByEmail.Id != id)
            {
                _logger.LogInformation("Update user data fail, new email already in use.");
                return Conflict(new { takenEmail = true });
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            userById.UpdateDate = DateTime.UtcNow;
            userById.Name = user.Name;
            userById.Email = user.Email;
            userById.Password = passwordHash;

            if (authenticatedUser.IsInRole("Admin"))
            {
                userById.Role = user.Role;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Update user data was succes.");
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            User? userById = await _context.TblUsers.FindAsync(id);
            if (userById == null)
            {
                _logger.LogInformation("Delete user data fail, user not found.");
                return NotFound(new { message = "User not found" });
            }

            var authenticatedUser = HttpContext.User;
            var userIdClaim = authenticatedUser.Claims.FirstOrDefault(
                claim => claim.Type == "userId"
            );
            if (userIdClaim == null)
            {
                _logger.LogInformation("Delete user forbidden.");
                return Forbid();
            }

            if (
                (userById.Id.ToString() != userIdClaim.Value)
                && !authenticatedUser.IsInRole("Admin")
            )
            {
                _logger.LogInformation("Delete user forbidden.");
                return Forbid();
            }
            _context.TblUsers.Remove(userById);
            _context.SaveChanges();
            _logger.LogInformation("User delete suscefully.");
            return NoContent();
        }

        public class QueryParameters
        {
            public required int Id { get; set; }
            public required string Password { get; set; }
        }

        [HttpGet("check")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PasswordCheck([FromQuery] QueryParameters parameters)
        {
            var userFound = await _context.TblUsers.FindAsync(parameters.Id);
            if (userFound == null)
            {
                _logger.LogInformation("Check password fail, user not found.");
                return Forbid();
            }
            var authenticatedUser = HttpContext.User;
            var userIdClaim = authenticatedUser.Claims.FirstOrDefault(
                claim => claim.Type == "userId"
            );
            if (userIdClaim == null)
            {
                _logger.LogInformation("Check password forbidden, claim meassing.");
                return Forbid();
            }
            if (userFound.Id.ToString() != userIdClaim.Value)
            {
                _logger.LogInformation("Check password forbidden, invalid user Id");
                return Forbid();
            }
            var passwordMatch = BCrypt.Net.BCrypt.Verify(parameters.Password, userFound.Password);
            if (!passwordMatch)
            {
                _logger.LogInformation("Check password forbidden, invalid password");
                return Forbid();
            }
            _logger.LogInformation("Password checked successfully");
            return Ok();
        }
    }
}
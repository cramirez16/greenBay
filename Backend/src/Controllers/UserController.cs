using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Src.Data;
using Src.Models.Dtos;
using Src.Services;
using Src.Models;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Src.Services.IServices;
using Src.Models.Specifications;
using Src.Repository.IRepository;
using src.Services.IServices;

namespace Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IJWTService _jwtService;
        private readonly IHassingService _hassingService;
        private readonly IMapper _automapper;
        private readonly IUserRepository _userRepo;

        public UserController(
            IJWTService jwtService,
            IHassingService hassingService,
            ILogger<UserController> logger,
            IMapper automapper,
            IUserRepository userRepo
        // if we dont use container dependency injection/service provider....
        // IUserRepository userRepo = new UserRepository(new GreenBayDbContext())
        // source: https://www.youtube.com/watch?v=oKyzza01rzA
        )
        {
            _jwtService = jwtService;
            _hassingService = hassingService;
            _logger = logger;
            _automapper = automapper;
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            // Given any parameters missing, the user can't sign in and the application
            // displays a message listing the missing parameters

            if (loginRequestDto.Email == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (email) in the request.");
                return BadRequest(new { missingEmail = true });
            }

            if (loginRequestDto.Password == null)
            {
                _logger.LogInformation("Login rejected, missing parameter (password) in the request.");
                return BadRequest(new { missingPassword = true });
            }

            // Read from the database the user data with the email = loginDto.Email 
            User? userByEmail = await _userRepo.FindUserByEmail(loginRequestDto.Email);

            // Given the usere mail dosent exists, 
            // the user can't sign in and the application displays a message about it
            if (userByEmail == null)
            {
                _logger.LogInformation("Login rejected, wrong Email.");
                return Unauthorized(new { wrongEmail = true });
            }

            // Given the password is not matching the one stored with the user email,
            // the user can't sign in and the application displays message that the password is wrong
            // if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, userByEmail.Password))
            if (!_hassingService.VerifyHash(loginRequestDto.Password, userByEmail.Password))
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
            var token = new LoginResponseDto
            {
                TokenKey = _jwtService.CreateToken(jwtPayLoad)
            };
            return Ok(token);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
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

            // cheking if the email address already exists in the database.
            var userByEmail = await _userRepo.FindUserByEmail(request.Email);

            if (userByEmail != null)
            {
                _logger.LogInformation("Register rejected, email already exists.");
                return Conflict(new { takenEmail = true });
            }

            // example of a hard dependency:
            // var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var passwordHash = _hassingService.CreateHash(request.Password);

            // stored generated pattern, when the new user is created in the database, EF
            // will update the value createdUser.Id property.
            // in User model ---> [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            // public int Id { get; set; }

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

            await _userRepo.AddUser(createdUser);
            await _userRepo.SaveToDbAsync();

            var createdUserDto = new RegisterResponseDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                Name = createdUser.Name,
                CreationDate = createdUser.CreationDate
            };
            _logger.LogInformation("New user registered.");
            string? actionName = nameof(GetUser); // http://localhost:5000/api/User/
            var routeValues = new { id = createdUser.Id }; // 8 (for example)
            var createdResource = createdUserDto;
            return CreatedAtAction(actionName, routeValues, createdResource);
            //                    ( string actionName, object routeValue, object value )
            // return status 201 and the data of the new user created (Id, Email, Name, CreationDate)
            // and location header with url to the location of the newly created user resource
            //
            // Response headers in swagger
            // access-control-allow-origin: * 
            // content-type: application/json; charset=utf-8 
            // date: Sat,14 Oct 2023 13:04:36 GMT 
            // location: http://localhost:5000/api/User/8 
            // server: Kestrel 
            // transfer-encoding: chunked 
            //
            // source: https://hamidmosalla.com/2017/03/29/asp-net-core-action-results-explained/
        }

        [HttpGet()] // localhost/api/User
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> List()
        {
            try
            {
                IEnumerable<User> users = await _userRepo.GetUsers();
                _logger.LogInformation("User list sent.");
                return Ok(_automapper.Map<IEnumerable<UserResponseDto>>(users));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{id}")] // localhost/api/User/{id}
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetUser([FromRoute] int id)
        {
            // id never is null, if id is not in the url, the request never arrive to this endpoint.
            // get localhost/api/User/{id} , no id => get localhost/api/User

            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/use-http-context?view=aspnetcore-7.0#user
            // "The HttpContext.User property is used to get or set the user, represented by ClaimsPrincipal, for the 
            // request. The ClaimsPrincipal is typically set by ASP.NET Core authentication."
            var authenticatedUser = HttpContext.User;
            var userIdClaim = authenticatedUser.Claims.FirstOrDefault(
                claim => claim.Type == "userId"
            );

            User? userById = await _userRepo.FindUserById(id);

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

            var userById = await _userRepo.FindUserById(id);

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

            var userByEmail = await _userRepo.FindUserByEmail(user.Email);

            if (userByEmail != null && userByEmail.Id != id)
            {
                _logger.LogInformation("Update user data fail, new email already in use.");
                return Conflict(new { takenEmail = true });
            }

            //var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var passwordHash = _hassingService.CreateHash(user.Password);

            userById.UpdateDate = DateTime.UtcNow;
            userById.Name = user.Name;
            userById.Email = user.Email;
            userById.Password = passwordHash;

            if (authenticatedUser.IsInRole("Admin"))
            {
                userById.Role = user.Role;
            }

            await _userRepo.SaveToDbAsync();
            _logger.LogInformation("Update user data was succes.");
            return Ok(new { userUpdated = true });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            User? userById = await _userRepo.FindUserById(id);
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
            _userRepo.DeleteUser(userById);
            _userRepo.SaveToDb();

            _logger.LogInformation("User delete suscefully.");
            return NoContent();
        }

        [HttpGet("check")]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PasswordCheck([FromQuery] QueryParameters parameters)
        {
            var userFound = await _userRepo.FindUserById(parameters.Id);
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
            //var passwordMatch = BCrypt.Net.BCrypt.Verify(parameters.Password, userFound.Password);
            var passwordMatch = _hassingService.VerifyHash(parameters.Password, userFound.Password);
            if (!passwordMatch)
            {
                _logger.LogInformation("Check password forbidden, invalid password");
                return Forbid();
            }
            _logger.LogInformation("Password checked successfully");
            return Ok(new { passwordChecked = true });
        }
    }
}
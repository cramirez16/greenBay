using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Src.Models;
using Src.Models.Dtos;
using Src.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Src.Services;

public class JWTService : IJWTService
{
    private readonly string _jwtKey;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public JWTService(string jwtKey, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _jwtKey = jwtKey;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public string CreateToken(JwtPayLoad jwtPayLoad)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", jwtPayLoad.UserId),
            new Claim("name", jwtPayLoad.Name),
            new Claim("email", jwtPayLoad.Email),
            new Claim(ClaimTypes.Role, jwtPayLoad.Role),
            new Claim("money", jwtPayLoad.Money)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public UserResponseDto? GetClaimsFromToken(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenSucceeded = handler.CanReadToken(token);

            if (tokenSucceeded)
            {
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.Claims != null && jwtToken.Claims.Any())
                {
                    return _mapper.Map<UserResponseDto>(jwtToken.Claims.ToList());
                }
            }
        }

        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext?.User;

        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            return _mapper.Map<UserResponseDto>(user.Claims.ToList());
        }

        return null;
    }

}
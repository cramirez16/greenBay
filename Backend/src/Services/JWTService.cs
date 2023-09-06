using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using src.Models;
using src.Models.Dtos;
using src.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace src.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public JWTService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _configuration = configuration;
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

        var secretKey = _configuration["JwtSettings:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public UserResponseDto? GetClaimsFromToken(string token)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext?.User;

        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            return _mapper.Map<UserResponseDto>(user.Claims.ToList());
        }

        return null;
    }

}
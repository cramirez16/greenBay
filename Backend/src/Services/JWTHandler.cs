using Microsoft.IdentityModel.Tokens;
using src.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services;

public class JWTHandler
{
    private readonly IConfiguration _configuration;

    public JWTHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(JwtPayLoad jwtPayLoad)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, jwtPayLoad.Role),
            new Claim("userId", jwtPayLoad.UserId),
            new Claim("name", jwtPayLoad.Name),
            new Claim("email", jwtPayLoad.Email),
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
}
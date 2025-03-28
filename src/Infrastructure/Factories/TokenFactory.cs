using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Factories;

public class TokenFactory : ITokenFactory
{
    private readonly JwtOption _jwtOption;

    public TokenFactory(IOptions<JwtOption> jwtOptions)
    {
        _jwtOption = jwtOptions.Value;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email), // Subject: the user's username.
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID: unique identifier for the token.
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOption.Issuer,
            _jwtOption.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtOption.ExpiryMinutes),
            signingCredentials: creds
        );
        var result = new JwtSecurityTokenHandler().WriteToken(token);
        return result;
    }
}
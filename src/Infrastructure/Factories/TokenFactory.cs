using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Factories;

public class TokenFactory(IOptions<JwtOption> jwtOptions) : ITokenFactory
{
    private readonly JwtOption _jwtOption = jwtOptions.Value;

    public string Create(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject: the user's id.
            new(JwtRegisteredClaimNames.Name, user.UserName), 
            new(JwtRegisteredClaimNames.Email, user.Email),
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
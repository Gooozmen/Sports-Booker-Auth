using System.Security.Claims;
using System.Text;
using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace CourtBooker.Auth.Infrastructure.Factories;

public class TokenFactory(IOptions<JwtOption> jwtOptions) : ITokenFactory
{
    private readonly JwtOption _jwtOption = jwtOptions.Value;

    public string Create(ApplicationUser user)
    {
        var key = AssemblySecurityKey();
        var credentials = AssemblySigningCredentials(key);
        var claims = AssemblyClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOption.ExpiryMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtOption.Issuer,
            Audience = _jwtOption.Audience
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }

    private SymmetricSecurityKey AssemblySecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
    }

    private SigningCredentials AssemblySigningCredentials(SymmetricSecurityKey key)
    {
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> AssemblyClaims(ApplicationUser user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject: the user's id.
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("email_verified", user.EmailConfirmed.ToString())
        ];
    }
}
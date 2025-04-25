using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Infrastructure.Factories;

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
        =>new(Encoding.UTF8.GetBytes(_jwtOption.Key));
    
    private SigningCredentials AssemblySigningCredentials(SymmetricSecurityKey key)
        =>new(key,SecurityAlgorithms.HmacSha256);
    
    private IEnumerable<Claim> AssemblyClaims(ApplicationUser user)
        =>
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject: the user's id.
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("email_verified", user.EmailConfirmed.ToString())
        ];
}
using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.Factories;
using CourtBooker.Auth.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace CourtBooker.Auth.Tests.Infra.Factories;

public class TokenFactoryTests
{
    private readonly JwtOption _jwtOptions;
    private readonly ITokenFactory _tokenFactory;

    public TokenFactoryTests()
    {
        _jwtOptions = new JwtOption
        {
            Key = "9rJvMk2fZ6cWpL1xYtBnUcDeHsQgAaSd",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };

        var optionsMock = Options.Create(_jwtOptions);
        _tokenFactory = new TokenFactory(optionsMock);
    }

    //TODO: after implement a token factory and understanding it implement propper tests, this is garbage currently haha
    [Fact]
    public void Create_ShouldGenerateValidJwtToken()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "juan",
            Email = "juan@test.com"
        };

        // Act
        var token = _tokenFactory.Create(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
    }
}
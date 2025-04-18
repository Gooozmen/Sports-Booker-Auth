using Application.Handlers;
using Application.Interfaces;
using Domain.Models;
using Moq;
using Shared.Commands;
using Shared.Enums;
using Shared.Queries;
using Shared.Responses;
using Shared.Responses.Auth;

namespace Tests.Application.CommandHandlers;

public class LoginCommandHandlerTests
{
    private readonly Mock<ILoginManager> _loginManagerMock;
    private readonly Mock<IApplicationUserManager> _userManagerMock;
    private readonly Mock<ITokenFactory> _tokenFactoryMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _loginManagerMock = new Mock<ILoginManager>();
        _userManagerMock = new Mock<IApplicationUserManager>();
        _tokenFactoryMock = new Mock<ITokenFactory>();

        _handler = new LoginCommandHandler(
            _loginManagerMock.Object,
            _userManagerMock.Object,
            _tokenFactoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthFailedResponse_WhenUserNotFound()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@email.com", Password = "123456"
            
        };
        
        _userManagerMock.Setup(x => 
                x.GetAsync(It.Is<UserQuery>(q => q.Email == command.Email && 
                                                 q.PropertyType == (int)IdentityPropertyTypes.UserEmail)))
            .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<AuthFailedResponse>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthFailedResponse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand{Email = "user@system.com", Password = "wrongpass"};
        var user = new ApplicationUser { Email = command.Email };

        _userManagerMock
            .Setup(x => x.GetAsync(It.IsAny<UserQuery>()))
            .ReturnsAsync(user);

        _loginManagerMock
            .Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<AuthFailedResponse>(result);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokenResponse_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand{Email = "login@success.com",Password = "correctpass"};
        var user = new ApplicationUser { Email = command.Email };
        var token = "token123";

        _userManagerMock
            .Setup(x => x.GetAsync(It.IsAny<UserQuery>()))
            .ReturnsAsync(user);

        _loginManagerMock
            .Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _tokenFactoryMock
            .Setup(x => x.Create(user))
            .Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var tokenResponse = Assert.IsType<LoginResponse>(result);
        Assert.True(tokenResponse.Success);
        Assert.Equal(token, tokenResponse.Bearer);
    }
}

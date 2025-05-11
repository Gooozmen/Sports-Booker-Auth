using Application.Handlers;
using Application.Interfaces;
using Domain.Models;
using Moq;
using Shared.Commands;
using Shared.Enums;
using Shared.Queries;
using Shared.Responses.Auth;
using Shared.Wrappers;

namespace Tests.Application.CommandHandlers;

public class LoginCommandHandlerTests
{
    private readonly LoginCommandHandler _handler;
    private readonly Mock<ILoginManager> _loginManagerMock;
    private readonly Mock<ITokenFactory> _tokenFactoryMock;
    private readonly Mock<IApplicationUserManager> _userManagerMock;

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
        Assert.IsType<LoginResponse>(result);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthFailedResponse_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand { Email = "user@system.com", Password = "wrongpass" };
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
        Assert.IsType<LoginResponse>(result);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokenResponse_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand { Email = "login@success.com", Password = "correctpass" };
        var user = new ApplicationUser { Email = command.Email };
        var bearer = "token123";
        var accessToken = new AccessToken(bearer);

        _userManagerMock
            .Setup(x => x.GetAsync(It.IsAny<UserQuery>()))
            .ReturnsAsync(user);

        _loginManagerMock
            .Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _tokenFactoryMock
            .Setup(x => x.Create(user))
            .Returns(bearer);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var loginResponse = Assert.IsType<LoginResponse>(result);
        Assert.True(loginResponse.IsSuccess);
        Assert.NotNull(loginResponse.AccessToken);
        Assert.Equal(accessToken.Bearer, loginResponse.AccessToken.Bearer);
    }
}
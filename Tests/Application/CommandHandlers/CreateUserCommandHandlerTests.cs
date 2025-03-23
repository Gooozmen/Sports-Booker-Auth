using Application.Builders;
using Application.CommandHandlers;
using Domain.Models;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Shared.Wrappers;
using Moq;
using Shared.Commands;

namespace Tests.Application.CommandHandlers;

public class CreateUserCommandHandlerTests
{
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<IApplicationUserManager> _mockApplicationUserManager;
    private readonly Mock<IApplicationUserBuilder> _mockUserBuilder;

    public CreateUserCommandHandlerTests()
    {
        // Create mock instances
        _mockApplicationUserManager = new Mock<IApplicationUserManager>();
        _mockUserBuilder = new Mock<IApplicationUserBuilder>();

        // Pass mocks to the command handler
        _handler = new CreateUserCommandHandler(_mockApplicationUserManager.Object, _mockUserBuilder.Object);
    }

    [Fact]
    public async Task ExecuteCreateAsync_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var command = new CreateUserCommand
            { Email = "test@example.com", Password = "Secure123!", PhoneNumber = "1234-2343" };
        var userModel = new ApplicationUser
            { UserName = command.Email, Email = command.Email, PhoneNumber = command.PhoneNumber };
        var wrapper = new ApplicationUserWrapper{ ApplicationUser = userModel, Password = command.Password };

        _mockUserBuilder.Setup(b => b.Apply(command)).Returns(userModel);

        _mockApplicationUserManager
            .Setup(m => m.CreateAsync(wrapper))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        _mockUserBuilder.Verify(b => b.Apply(command), Times.Once); // Ensure Apply() was called once
        _mockApplicationUserManager.Verify(m => m.CreateAsync(wrapper),
            Times.Once); // Ensure CreateUserAsync() was called once
    }

    [Fact]
    public async Task ExecuteCreateAsync_ShouldReturnFailure_WhenUserCreationFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };
        var userModel = new ApplicationUser { UserName = command.Email, Email = command.Email };
        var wrapper = new ApplicationUserWrapper{ ApplicationUser = userModel, Password = command.Password };

        _mockUserBuilder.Setup(b => b.Apply(command)).Returns(userModel);

        _mockApplicationUserManager
            .Setup(m => m.CreateAsync(wrapper))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Single(result.Errors);
        Assert.Equal("User creation failed", result.Errors.First().Description);
    }

    [Fact]
    public async Task ExecuteCreateAsync_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };

        _mockApplicationUserManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUserWrapper>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
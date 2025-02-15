using Application.Builders;
using Application.CommandHandlers;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Commands.ApplicationUser;


namespace Tests.Application.CommandHandlers;


public class ApplicationUserCommandHandlerTests
{
    private readonly Mock<IApplicationUserManager> _mockApplicationUserManager;
    private readonly Mock<IApplicationUserBuilder> _mockUserBuilder;
    private readonly ApplicationUserCommandHandler _handler;

    public ApplicationUserCommandHandlerTests()
    {
        // Create mock instances
        _mockApplicationUserManager = new Mock<IApplicationUserManager>();
        _mockUserBuilder = new Mock<IApplicationUserBuilder>();

        // Pass mocks to the command handler
        _handler = new ApplicationUserCommandHandler(_mockApplicationUserManager.Object, _mockUserBuilder.Object);
    }

    [Fact]
    public async Task ExecuteCreateAsync_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!",PhoneNumber = "1234-2343"};
        var userModel = new ApplicationUser { UserName = command.Email, Email = command.Email, PhoneNumber = command.PhoneNumber };

        _mockUserBuilder.Setup(b => b.Apply(command)).Returns(userModel);

        _mockApplicationUserManager
            .Setup(m => m.CreateUserAsync(userModel, command.Password))
            .ReturnsAsync(IdentityResult.Success); 

        // Act
        var result = await _handler.ExecuteCreateAsync(command);

        // Assert
        Assert.True(result.Succeeded);
        _mockUserBuilder.Verify(b => b.Apply(command), Times.Once); // Ensure Apply() was called once
        _mockApplicationUserManager.Verify(m => m.CreateUserAsync(userModel, command.Password), Times.Once); // Ensure CreateUserAsync() was called once
    }

    [Fact]
    public async Task ExecuteCreateAsync_ShouldReturnFailure_WhenUserCreationFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };
        var userModel = new ApplicationUser { UserName = command.Email, Email = command.Email };

        _mockUserBuilder.Setup(b => b.Apply(command)).Returns(userModel);

        _mockApplicationUserManager
            .Setup(m => m.CreateUserAsync(userModel, command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" })); 

        // Act
        var result = await _handler.ExecuteCreateAsync(command);

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
            .Setup(m => m.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ThrowsAsync(new System.Exception("Unexpected error")); 

        // Act & Assert
        await Assert.ThrowsAsync<System.Exception>(() => _handler.ExecuteCreateAsync(command));
    }
}

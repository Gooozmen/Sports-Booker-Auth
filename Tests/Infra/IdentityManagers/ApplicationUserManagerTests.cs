using Domain.Models;
using FluentAssertions;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Infra.IdentityManagers;

public class ApplicationUserManagerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly ApplicationUserManager _applicationUserManager;

    public ApplicationUserManagerTests()
    {
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), 
            null, null, null, null, null, null, null, null
        );

        _applicationUserManager = new ApplicationUserManager(_mockUserManager.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
        var password = "Test@123";

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationUserManager.CreateUserAsync(user, password);

        // Assert
        result.Should().Be(IdentityResult.Success);
        _mockUserManager.Verify(um => um.CreateAsync(user, password), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnFailure_WhenUserCreationFails()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
        var password = "Test@123";

        var identityErrors = new[] { new IdentityError { Description = "Error creating user" } };
        var failedResult = IdentityResult.Failed(identityErrors);

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _applicationUserManager.CreateUserAsync(user, password);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle()
              .Which.Description
              .Should().Be("Error creating user");
        
        _mockUserManager.Verify(um => um.CreateAsync(user, password), Times.Once);
    }
}

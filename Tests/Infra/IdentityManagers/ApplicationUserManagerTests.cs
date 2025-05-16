using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Moq;
using CourtBooker.Auth.Shared.Wrappers;

namespace CourtBooker.Auth.Tests.Infra.IdentityManagers;

public class ApplicationUserManagerTests
{
    private readonly ApplicationUserManager _applicationUserManager;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

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
        var wrapper = new ApplicationUserWrapper { ApplicationUser = user, Password = password };

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationUserManager.CreateAsync(wrapper);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        _mockUserManager.Verify(um => um.CreateAsync(user, password), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnFailure_WhenUserCreationFails()
    {
        // Arrange
        var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
        var password = "Test@123";
        var wrapper = new ApplicationUserWrapper { ApplicationUser = user, Password = password };

        var identityErrors = new[] { new IdentityError { Description = "Error creating user" } };
        var failedResult = IdentityResult.Failed(identityErrors);

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _applicationUserManager.CreateAsync(wrapper);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, x => x.Description == "Error creating user");

        _mockUserManager.Verify(um => um.CreateAsync(user, password), Times.Once);
    }
}
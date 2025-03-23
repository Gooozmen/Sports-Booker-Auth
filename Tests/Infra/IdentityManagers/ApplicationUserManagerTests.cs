using Domain.Models;
using FluentAssertions;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Wrappers;

namespace Tests.Infra.IdentityManagers;

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
        var wrapper = new ApplicationUserWrapper{ApplicationUser = user, Password = password};

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationUserManager.CreateAsync(wrapper);

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
        var wrapper = new ApplicationUserWrapper{ApplicationUser = user, Password = password};

        var identityErrors = new[] { new IdentityError { Description = "Error creating user" } };
        var failedResult = IdentityResult.Failed(identityErrors);

        _mockUserManager.Setup(um => um.CreateAsync(user, password))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _applicationUserManager.CreateAsync(wrapper);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Description
            .Should().Be("Error creating user");

        _mockUserManager.Verify(um => um.CreateAsync(user, password), Times.Once);
    }
}
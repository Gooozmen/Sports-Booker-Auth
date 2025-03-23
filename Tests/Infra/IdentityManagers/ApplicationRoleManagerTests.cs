using Domain.Models;
using FluentAssertions;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Infra.IdentityManagers;

public class ApplicationRoleManagerTests
{
    private readonly ApplicationRoleManager _applicationRoleManager;
    private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;

    public ApplicationRoleManagerTests()
    {
        var roleStore = new Mock<IRoleStore<ApplicationRole>>();

        _mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
            roleStore.Object,
            null, null, null, null
        );

        _applicationRoleManager = new ApplicationRoleManager(_mockRoleManager.Object);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
    {
        // Arrange
        var role = new ApplicationRole { Name = "userRole", Active = true };

        _mockRoleManager.Setup(rm => rm.CreateAsync(role))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _applicationRoleManager.CreateAsync(role);

        // Assert
        result.Should().Be(IdentityResult.Success);
        _mockRoleManager.Verify(rm => rm.CreateAsync(role), Times.Once);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldReturnFailure_WhenRoleCreationFails()
    {
        // Arrange
        var role = new ApplicationRole { Name = "userRole", Active = true };

        var identityErrors = new[] { new IdentityError { Description = "Error creating Role" } };
        var failedResult = IdentityResult.Failed(identityErrors);

        _mockRoleManager.Setup(rm => rm.CreateAsync(role))
            .ReturnsAsync(failedResult);

        // Act
        var result = await _applicationRoleManager.CreateAsync(role);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Description
            .Should().Be("Error creating Role");

        _mockRoleManager.Verify(rm => rm.CreateAsync(role), Times.Once);
    }
}
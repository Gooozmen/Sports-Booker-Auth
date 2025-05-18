using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Application.Handlers;
using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using CourtBooker.Auth.Shared.Commands;

namespace CourtBooker.Auth.Tests.Application.CommandHandlers;

public class CreateRoleCommandHandlerTests
{
    private readonly Mock<IApplicationRoleManager> _roleManagerMock;
    private readonly Mock<IApplicationRoleBuilder> _roleBuilderMock;
    private readonly CreateRoleCommandHandler _handler;

    public CreateRoleCommandHandlerTests()
    {
        _roleManagerMock = new Mock<IApplicationRoleManager>();
        _roleBuilderMock = new Mock<IApplicationRoleBuilder>();

        _handler = new CreateRoleCommandHandler(_roleManagerMock.Object, _roleBuilderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenRoleIsCreated()
    {
        // Arrange
        var command = new CreateRoleCommand { Name = "Admin" };
        var applicationRole = new ApplicationRole { Name = command.Name };

        _roleBuilderMock
            .Setup(b => b.Apply(command))
            .Returns(applicationRole);

        _roleManagerMock
            .Setup(m => m.CreateAsync(applicationRole))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        _roleBuilderMock.Verify(b => b.Apply(command), Times.Once);
        _roleManagerMock.Verify(m => m.CreateAsync(applicationRole), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRoleCreationFails()
    {
        // Arrange
        var command = new CreateRoleCommand { Name = "Manager" };
        var applicationRole = new ApplicationRole { Name = command.Name };
        var identityError = new IdentityError { Description = "Role already exists" };

        _roleBuilderMock
            .Setup(b => b.Apply(command))
            .Returns(applicationRole);

        _roleManagerMock
            .Setup(m => m.CreateAsync(applicationRole))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Single(result.Errors);
        Assert.Equal("Role already exists", result.Errors.First().Description);
    }
}




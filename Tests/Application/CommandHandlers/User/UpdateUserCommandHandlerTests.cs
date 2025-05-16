using CourtBooker.Auth.Application.Handlers;
using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using CourtBooker.Auth.Shared.Commands;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Queries;
using CourtBooker.Auth.Shared.Wrappers;

namespace CourtBooker.Auth.Tests.Application.CommandHandlers;

public class UpdateUserCommandHandlerTests
{
    private readonly UpdateUserCommandHandler _handler;
    private readonly Mock<IApplicationUserManager> _userManagerMock;

    public UpdateUserCommandHandlerTests()
    {
        _userManagerMock = new Mock<IApplicationUserManager>();
        _handler = new UpdateUserCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand { Id = userId, Email = "updated@email.com", PhoneNumber = "555-5555" };
        var user = new ApplicationUser { Id = userId, Email = "original@email.com", UserName = "original@email.com" };

        _userManagerMock
            .Setup(x => x.GetAsync(It.Is<UserQuery>(q =>
                q.Id == command.Id.ToString() &&
                q.PropertyType == (int)IdentityPropertyTypes.UserId)))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.Is<ApplicationUserWrapper>(w => w.ApplicationUser.Email == command.Email)))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedResult_WhenUserNotFound()
    {
        // Arrange
        var command = new UpdateUserCommand { Id = Guid.NewGuid() };

        _userManagerMock
            .Setup(x => x.GetAsync(It.IsAny<UserQuery>()))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Description == "User not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUpdateFails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand { Id = userId };
        var user = new ApplicationUser { Id = userId, Email = "test@test.com" };

        _userManagerMock
            .Setup(x => x.GetAsync(It.IsAny<UserQuery>()))
            .ReturnsAsync(user);

        var identityError = new IdentityError { Description = "Update failed" };

        _userManagerMock
            .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUserWrapper>()))
            .ReturnsAsync(IdentityResult.Failed(identityError));

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Description == "Update failed");
    }
}
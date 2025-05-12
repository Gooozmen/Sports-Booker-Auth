using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Application.Handlers;
using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using Moq;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Queries;
using CourtBooker.Auth.Shared.Responses;

namespace CourtBooker.Auth.Tests.Application.CommandHandlers;

public class GetUserQueryHandlerTests
{
    private readonly GetUserQueryHandler _handler;
    private readonly Mock<IApplicationUserBuilder> _mockApplicationBuilder;
    private readonly Mock<IApplicationUserManager> _mockApplicationUserManager;

    public GetUserQueryHandlerTests()
    {
        _mockApplicationBuilder = new Mock<IApplicationUserBuilder>();
        _mockApplicationUserManager = new Mock<IApplicationUserManager>();
        _handler = new GetUserQueryHandler(_mockApplicationUserManager.Object, _mockApplicationBuilder.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUser()
    {
        //arrange
        var userId = Guid.NewGuid();
        var request = new UserQuery(userId.ToString(), (int)IdentityPropertyTypes.UserId);
        var applicationUser = new ApplicationUser
        {
            Id = userId,
            UserName = "juan@example.com",
            Email = "juan@example.com",
            EmailConfirmed = true,
            PhoneNumber = "123-456-7890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            LockoutEnabled = false,
            Active = true
        };

        var userResponse = new UserResponse
        {
            Id = applicationUser.Id,
            Username = applicationUser.UserName,
            Email = applicationUser.Email,
            EmailConfirmed = applicationUser.EmailConfirmed,
            PhoneNumber = applicationUser.PhoneNumber,
            PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
            TwoFactorEnabled = applicationUser.TwoFactorEnabled,
            Active = applicationUser.Active,
            LockoutEnabled = applicationUser.LockoutEnabled,
            IsSuccess = true
        };

        _mockApplicationUserManager.Setup(c =>
                c.GetAsync(It.Is<UserQuery>(w =>
                    w.Id == userId.ToString() && w.PropertyType == (int)IdentityPropertyTypes.UserId)))
            .ReturnsAsync(applicationUser);

        _mockApplicationBuilder
            .Setup(b => b.Apply(It.IsAny<ApplicationUser>())).Returns(userResponse);

        var result = await _handler.Handle(request);

        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(UserResponse));
        Assert.Equal(applicationUser.Id, result.Id);
        Assert.Equal(applicationUser.UserName, result.Username);
        Assert.Equal(applicationUser.Email, result.Email);
        Assert.Equal(applicationUser.EmailConfirmed, result.EmailConfirmed);
        Assert.Equal(applicationUser.PhoneNumber, result.PhoneNumber);
        Assert.Equal(applicationUser.PhoneNumberConfirmed, result.PhoneNumberConfirmed);
        Assert.Equal(applicationUser.TwoFactorEnabled, result.TwoFactorEnabled);
        Assert.Equal(applicationUser.Active, result.Active);
        Assert.Equal(applicationUser.LockoutEnabled, result.LockoutEnabled);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUser()
    {
        //arrange
        var userEmail = "juan@example.com";
        var userId = Guid.NewGuid();
        var request = new UserQuery(userEmail, (int)IdentityPropertyTypes.UserEmail);
        var applicationUser = new ApplicationUser
        {
            Id = userId,
            UserName = "juan@example.com",
            Email = "juan@example.com",
            EmailConfirmed = true,
            PhoneNumber = "123-456-7890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            LockoutEnabled = false,
            Active = true
        };

        var userResponse = new UserResponse
        {
            Id = applicationUser.Id,
            Username = applicationUser.UserName,
            Email = applicationUser.Email,
            EmailConfirmed = applicationUser.EmailConfirmed,
            PhoneNumber = applicationUser.PhoneNumber,
            PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
            TwoFactorEnabled = applicationUser.TwoFactorEnabled,
            Active = applicationUser.Active,
            LockoutEnabled = applicationUser.LockoutEnabled,
            IsSuccess = true
        };

        _mockApplicationUserManager.Setup(c =>
                c.GetAsync(It.Is<UserQuery>(w =>
                    w.Email == userEmail && w.PropertyType == (int)IdentityPropertyTypes.UserEmail)))
            .ReturnsAsync(applicationUser);

        _mockApplicationBuilder
            .Setup(b => b.Apply(It.IsAny<ApplicationUser>())).Returns(userResponse);

        var result = await _handler.Handle(request);

        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(UserResponse));
        Assert.Equal(applicationUser.Id, result.Id);
        Assert.Equal(applicationUser.UserName, result.Username);
        Assert.Equal(applicationUser.Email, result.Email);
        Assert.Equal(applicationUser.EmailConfirmed, result.EmailConfirmed);
        Assert.Equal(applicationUser.PhoneNumber, result.PhoneNumber);
        Assert.Equal(applicationUser.PhoneNumberConfirmed, result.PhoneNumberConfirmed);
        Assert.Equal(applicationUser.TwoFactorEnabled, result.TwoFactorEnabled);
        Assert.Equal(applicationUser.Active, result.Active);
        Assert.Equal(applicationUser.LockoutEnabled, result.LockoutEnabled);
    }
}
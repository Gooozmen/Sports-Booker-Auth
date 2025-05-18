using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Shared.Commands;

namespace CourtBooker.Auth.Tests.Application.Builders;

public class ApplicationUserBuilderTests
{
    private readonly ApplicationUserBuilder _applicationUserBuilder;

    public ApplicationUserBuilderTests()
    {
        _applicationUserBuilder = new ApplicationUserBuilder();
    }

    [Fact]
    public void BuilderShouldReturnApplicationUserModel()
    {
        //arrange
        var cmd = new CreateUserCommand { Email = "pepito@gmail.com", Password = "123456" };

        //act
        var result = _applicationUserBuilder.Apply(cmd);

        //assert

        Assert.True(result.GetType() == typeof(ApplicationUser));
    }

    [Fact]
    public void BuilderShouldMapCommandValuesToUserModel()
    {
        //arrange
        var cmd = new CreateUserCommand { Email = "test@test.com", Password = "pass2345", PhoneNumber = "0123456789" };

        //act
        var result = _applicationUserBuilder.Apply(cmd);

        //assert
        Assert.True(result.GetType() == typeof(ApplicationUser));
        Assert.True(result.Email == cmd.Email);
        Assert.True(result.UserName == cmd.Email);
        Assert.True(result.PhoneNumber == cmd.PhoneNumber);
        Assert.True(result.Active);
    }

    [Fact]
    public void PhoneNumberSholdBeNullIfNotDefined()
    {
        //arrange
        var cmd = new CreateUserCommand { Email = "pepito@gmail.com", Password = "123456" };

        //act
        var result = _applicationUserBuilder.Apply(cmd);

        //assert
        Assert.True(result.PhoneNumber == null);
    }
    
    [Fact]
    public void BuilderShouldMapUpdateCommandToApplicationUser()
    {
        // Arrange
        var cmd = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Email = "updated@test.com",
            PhoneNumber = "999888777"
        };

        // Act
        var result = _applicationUserBuilder.Apply(cmd);

        // Assert
        Assert.Equal(cmd.Id, result.Id);
        Assert.Equal(cmd.Email, result.Email);
        Assert.Equal(cmd.Email, result.UserName);
        Assert.Equal(cmd.PhoneNumber, result.PhoneNumber);
    }

    
    [Fact]
    public void BuilderShouldMapApplicationUserToUserResponse()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "user@email.com",
            UserName = "user@email.com",
            PhoneNumber = "123456",
            Active = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = true,
            LockoutEnabled = false
        };

        // Act
        var result = _applicationUserBuilder.Apply(user);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.UserName, result.Username);
        Assert.Equal(user.PhoneNumber, result.PhoneNumber);
        Assert.Equal(user.Active, result.Active);
        Assert.Equal(user.EmailConfirmed, result.EmailConfirmed);
        Assert.Equal(user.PhoneNumberConfirmed, result.PhoneNumberConfirmed);
        Assert.Equal(user.TwoFactorEnabled, result.TwoFactorEnabled);
        Assert.Equal(user.LockoutEnabled, result.LockoutEnabled);
        Assert.True(result.IsSuccess);
    }
}
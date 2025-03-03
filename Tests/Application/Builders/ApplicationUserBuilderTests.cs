using Application.Builders;
using Domain.Models;
using Shared.Commands;

namespace Tests.Application.Builders;

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
        var cmd = new CreateUserCommand{Email = "pepito@gmail.com", Password = "123456"};

        //act
        var result = _applicationUserBuilder.Apply(cmd);
        
        //assert

        Assert.True(result.GetType() == typeof(ApplicationUser));
    }
    
    [Fact]
    public void BuilderShouldMapCommandValuesToUserModel()
    {
        //arrange
        var cmd = new CreateUserCommand {Email = "test@test.com", Password = "pass2345", PhoneNumber = "0123456789"};

        //act
        var result = _applicationUserBuilder.Apply(cmd);
        
        //assert
        Assert.True(result.GetType() == typeof(ApplicationUser));
        Assert.True(result.Email == cmd.Email);
        Assert.True(result.UserName == cmd.Email);
        Assert.True(result.PhoneNumber == cmd.PhoneNumber);
        Assert.True(result.Active == true);

    }
    
    [Fact]
    public void PhoneNumberSholdBeNullIfNotDefined()
    {
        //arrange
        var cmd = new CreateUserCommand {Email = "pepito@gmail.com", Password = "123456"};

        //act
        var result = _applicationUserBuilder.Apply(cmd);
        
        //assert
        Assert.True(result.PhoneNumber == null);

    }
}
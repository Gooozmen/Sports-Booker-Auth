using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Shared.Commands;

namespace CourtBooker.Auth.Tests.Application.Builders;

public class ApplicationRoleBuilderTests
{
    private readonly ApplicationRoleBuilder _roleBuilder;
    
    public ApplicationRoleBuilderTests()
    {
        _roleBuilder = new ApplicationRoleBuilder();
    }

    [Fact]
    public void BuilderShouldReturnApplicationUserModel()
    {
        //arrange
        var cmd = new CreateRoleCommand() { Name = "RoleName"};

        //act
        var result = _roleBuilder.Apply(cmd);

        //assert

        Assert.True(result.GetType() == typeof(ApplicationRole));
    }

    [Fact]
    public void BuilderShouldMapCommandValuesToUserModel()
    {
        //arrange
        var cmd = new CreateRoleCommand() { Name = "RoleName"};

        //act
        var result = _roleBuilder.Apply(cmd);

        //assert
        Assert.True(result.GetType() == typeof(ApplicationRole));
        Assert.True(result.Name == cmd.Name);
        Assert.True(result.ConcurrencyStamp != null &&  result.ConcurrencyStamp.Length > 0);
        Assert.True(result.Active);
    }
}
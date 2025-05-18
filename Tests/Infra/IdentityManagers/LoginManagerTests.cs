using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CourtBooker.Auth.Tests.Infra.IdentityManagers;

public class LoginManagerTests
{
    Mock<UserManager<ApplicationUser>> _userManagerMock;
    LoginManager _loginManager;

    public LoginManagerTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );

        _loginManager = new LoginManager(_userManagerMock.Object);
    }

    [Fact]
    public async Task CheckPasswordAsync_ValidPassword_ReturnsTrue()
    {
        var user = new ApplicationUser { UserName = "test" };
        var password = "testlajd!#*&";

        _userManagerMock.Setup(c => c.CheckPasswordAsync(user, password)).ReturnsAsync(true);
        
        var result =  await _loginManager.CheckPasswordAsync(user, password);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckPasswordAsync_InvalidPassword_ReturnsFalse()
    {
        var user = new ApplicationUser { UserName = "test" };
        var password = "testlajd!#*&";

        _userManagerMock.Setup(c => c.CheckPasswordAsync(user, password)).ReturnsAsync(false);
        
        var result =  await _loginManager.CheckPasswordAsync(user, password);
        
        Assert.False(result);
    }
}
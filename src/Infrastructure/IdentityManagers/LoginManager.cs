using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Infrastructure.IdentityManagers;

public class LoginManager
    (
        UserManager<ApplicationUser> userManager
    ) 
    : ILoginManager
{
    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password);
    }
}


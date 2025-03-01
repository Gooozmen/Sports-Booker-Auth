using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Interfaces;

namespace Infrastructure.IdentityManagers;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserManager(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user,string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user,password);
        return result;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);
        return result;
    }
}

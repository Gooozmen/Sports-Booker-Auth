using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Wrappers;
using Shared.Queries;

namespace Infrastructure.IdentityManagers;
 
public class ApplicationUserManager(UserManager<ApplicationUser> userManager) : IApplicationUserManager
{
    public async Task<IdentityResult> CreateAsync(ApplicationUserWrapper wrapper)
    {
        var result = await userManager.CreateAsync(wrapper.ApplicationUser, wrapper.Password);
        return result;
    }

    public async Task<ApplicationUser?> GetAsync(UserQuery query)
        => query.PropertyType switch
        {
            (int)IdentityPropertyTypes.UserId => await userManager.FindByIdAsync(query.Id),
            (int)IdentityPropertyTypes.UserLogin => await userManager.FindByLoginAsync(query.LoginProvider, query.ProviderKey),
            (int)IdentityPropertyTypes.UserEmail => await userManager.FindByEmailAsync(query.Email),
            (int)IdentityPropertyTypes.UserName => await userManager.FindByNameAsync(query.UserName)
        };

    public async Task<IdentityResult> UpdateAsync(ApplicationUserWrapper wrapper) 
        => await userManager.UpdateAsync(wrapper.ApplicationUser);
}


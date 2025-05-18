using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Wrappers;
using CourtBooker.Auth.Shared.Queries;

namespace CourtBooker.Auth.Infrastructure.IdentityManagers;
 
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
            (int)IdentityPropertyTypes.UserId => await userManager.FindByIdAsync(query.Id!),
            (int)IdentityPropertyTypes.UserEmail => await userManager.FindByEmailAsync(query.Email!),
            (int)IdentityPropertyTypes.UserName => await userManager.FindByNameAsync(query.UserName!),
            _ => throw new ArgumentOutOfRangeException()
        };

    public async Task<IdentityResult> UpdateAsync(ApplicationUserWrapper wrapper) 
        => await userManager.UpdateAsync(wrapper.ApplicationUser);
}


using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Wrappers;

namespace Infrastructure.IdentityManagers;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserManager(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUserWrapper model)
    {
        var result = await _userManager.CreateAsync(model.ApplicationUser, model.Password);
        return result;
    }

    public async Task<ApplicationUser?> GetAsync(UserQuery query)
    {
        return query.GetPropertyType() switch
        {
            (int)IdentityPropertyTypes.UserId => await _userManager.FindByIdAsync(query.Id),
            (int)IdentityPropertyTypes.UserLogin => await _userManager.FindByLoginAsync(query.LoginProvider, query.ProviderKey),
            (int)IdentityPropertyTypes.UserEmail => await _userManager.FindByEmailAsync(query.Email),
            (int)IdentityPropertyTypes.UserName => await _userManager.FindByNameAsync(query.UserName)
        };
    }
}

public interface IApplicationUserManager :
    ICommandManager<ApplicationUserWrapper,IdentityResult>,
    IQueryableManager<ApplicationUser,UserQuery>
{
}
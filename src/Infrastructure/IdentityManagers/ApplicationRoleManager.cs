using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Wrappers;

namespace Infrastructure.IdentityManagers;

public class ApplicationRoleManager : IApplicationRoleManager
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationRoleManager(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationRole model)
    {
        var identityResult = await _roleManager.CreateAsync(model);
        return identityResult;
    }
    public async Task<ApplicationRole?> GetAsync(ApplicationRoleQuery data)
    {
        return data.GetPropertyType() switch
        {
            (int)IdentityPropertyTypes.RoleName => await _roleManager.FindByNameAsync(data.Name),
            (int)IdentityPropertyTypes.RoleId => await _roleManager.FindByIdAsync(data.Id)
        };
    }
}

public interface IApplicationRoleManager : 
    ICommandManager<ApplicationRole,IdentityResult>,
    IQueryableManager<ApplicationRole, ApplicationRoleQuery>
{
}
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Queries;

namespace Infrastructure.IdentityManagers;

public class ApplicationRoleManager(RoleManager<ApplicationRole> roleManager) : IApplicationRoleManager
{
    public async Task<IdentityResult> CreateAsync(ApplicationRole model)
    {
        var identityResult = await roleManager.CreateAsync(model);
        return identityResult;
    }

    public Task<IdentityResult> UpdateAsync(ApplicationRole model)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationRole?> GetAsync(ApplicationRoleQuery data)
    {
        return data.PropertyType switch
        {
            (int)IdentityPropertyTypes.RoleName => await roleManager.FindByNameAsync(data.Name),
            (int)IdentityPropertyTypes.RoleId => await roleManager.FindByIdAsync(data.Id),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}


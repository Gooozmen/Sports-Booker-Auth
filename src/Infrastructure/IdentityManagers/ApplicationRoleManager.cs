using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityManagers;

public class ApplicationRoleManager : IApplicationRoleManager
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationRoleManager(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
    {
        var identityResult = await _roleManager.CreateAsync(role);
        return identityResult;
    }
}
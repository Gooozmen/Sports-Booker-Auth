using Application.Builders;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationRole;

namespace Application.CommandHandlers;

public class ApplicationRoleCommandHandler : IApplicationRoleCommandHandler
{
    private readonly IApplicationRoleManager _roleManager;
    private readonly IApplicationRoleBuilder _roleBuilder;
    
    public ApplicationRoleCommandHandler
    (
        IApplicationRoleManager roleManager, 
        IApplicationRoleBuilder roleBuilder
    )
    {
        _roleManager = roleManager;
        _roleBuilder = roleBuilder;
    }
    
    public async Task<IdentityResult> ExecuteCreateAsync(CreateRoleCommand cmd)
    {
        var dataModel = _roleBuilder.Apply(cmd);
        var result = await _roleManager.CreateRoleAsync(dataModel);
        return result;
    }
}

public interface IApplicationRoleCommandHandler
{
    Task<IdentityResult> ExecuteCreateAsync( CreateRoleCommand cmd);
}
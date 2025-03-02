using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Commands.ApplicationRole;
using Shared.Interfaces;

namespace Application.CommandHandlers;

public class ApplicationRoleCommandHandler : BaseCommandHandler<ICommand>
{
    private readonly IApplicationRoleManager _roleManager;
    private readonly IBuilder<CreateRoleCommand,ApplicationRole> _roleBuilder;
    
    public ApplicationRoleCommandHandler
    (
        IApplicationRoleManager roleManager, 
        IBuilder<CreateRoleCommand,ApplicationRole> roleBuilder
    )
    {
        _roleManager = roleManager;
        _roleBuilder = roleBuilder;
    }
    
    public async Task<Object> Handle(ICommand command)
    {
        return command switch
        {
            CreateRoleCommand createRoleCommand => await ExecuteCreateAsync(createRoleCommand),
            _ => new NotDefinedCommand()
        };
    }
    
    private async Task<IdentityResult> ExecuteCreateAsync(CreateRoleCommand cmd)
    {
        var dataModel = _roleBuilder.Apply(cmd);
        var result = await _roleManager.CreateRoleAsync(dataModel);
        return result;
    }
    
}

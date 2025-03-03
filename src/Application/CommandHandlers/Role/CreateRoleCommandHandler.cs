using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;

namespace Application.CommandHandlers;

public class CreateRoleCommandHandler
    (
        IApplicationRoleManager roleManager,
        IBuilder<CreateRoleCommand, ApplicationRole> roleBuilder
    )
    : IRequestHandler<CreateRoleCommand, IdentityResult>
{
    
    public async Task<IdentityResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var dataModel = roleBuilder.Apply(command);
        var result = await roleManager.CreateRoleAsync(dataModel);
        return result;
    }
}

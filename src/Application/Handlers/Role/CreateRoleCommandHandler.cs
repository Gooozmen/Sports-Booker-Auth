using Application.Builders;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;

namespace Application.Handlers;

public class CreateRoleCommandHandler(
    IApplicationRoleManager roleManager,
    IApplicationRoleBuilder roleBuilder
)
    : IRequestHandler<CreateRoleCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var dataModel = roleBuilder.Apply(command);
        var result = await roleManager.CreateAsync(dataModel);
        return result;
    }
}
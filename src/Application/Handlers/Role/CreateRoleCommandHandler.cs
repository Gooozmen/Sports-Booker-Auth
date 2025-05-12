using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CourtBooker.Auth.Shared.Commands;

namespace CourtBooker.Auth.Application.Handlers;

public class CreateRoleCommandHandler(
    IApplicationRoleManager roleManager,
    IApplicationRoleBuilder roleBuilder
)
    : IRequestHandler<CreateRoleCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        var dataModel = roleBuilder.Apply(command);
        var result = await roleManager.CreateAsync(dataModel);
        return result;
    }
}
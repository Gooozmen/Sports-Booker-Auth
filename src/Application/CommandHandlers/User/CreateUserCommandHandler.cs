using Application.Builders;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationUser;

namespace Application.CommandHandlers;

public class CreateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    IBuilder<CreateUserCommand, ApplicationUser> userBuilder)
    : ICommandHandler<CreateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var dataModel = userBuilder.Apply(command);
        var result = await applicationUserManager.CreateUserAsync(dataModel,command.Password);
        return result;
    }
}
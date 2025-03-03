using Application.Builders;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;

namespace Application.CommandHandlers;

public class CreateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    IApplicationUserBuilder userBuilder)
    : IRequestHandler<CreateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var dataModel = userBuilder.Apply(command);
        var result = await applicationUserManager.CreateUserAsync(dataModel,command.Password);
        return result;
    }
}
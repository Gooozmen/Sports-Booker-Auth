using Application.Builders;
using Infrastructure.IdentityManagers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Wrappers;

namespace Application.CommandHandlers;

public class CreateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    IApplicationUserBuilder userBuilder)
    : IRequestHandler<CreateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var dataModel = userBuilder.Apply(command);
        var wrapper = new ApplicationUserWrapper{ApplicationUser = dataModel, Password = command.Password};
        var result = await applicationUserManager.CreateAsync(wrapper);
        return result;
    }
}
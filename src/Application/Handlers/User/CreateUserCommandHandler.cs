using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CourtBooker.Auth.Shared.Commands;
using CourtBooker.Auth.Shared.Wrappers;

namespace CourtBooker.Auth.Application.Handlers;

public class CreateUserCommandHandler(
    IApplicationUserManager applicationUserManager,
    IApplicationUserBuilder userBuilder)
    : IRequestHandler<CreateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var dataModel = userBuilder.Apply(command);
        var wrapper = new ApplicationUserWrapper { ApplicationUser = dataModel, Password = command.Password };
        var result = await applicationUserManager.CreateAsync(wrapper);
        return result;
    }
}
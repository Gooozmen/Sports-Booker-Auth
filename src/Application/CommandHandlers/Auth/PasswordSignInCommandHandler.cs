using Application.Factories;
using Infrastructure.IdentityManagers;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Enums;
using Shared.Responses;
using Shared.Wrappers;

namespace Application.CommandHandlers;

public class PasswordSignInCommandHandler(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    IPasswordSignInResponseFactory responseFactory,
    ITokenFactory tokenFactory
)
    : IRequestHandler<PasswordSignInCommand, SignInResponseBase>
{
    public async Task<SignInResponseBase> Handle(PasswordSignInCommand command, CancellationToken cancellationToken)
    {
        var token = string.Empty;
        var userQuery = new UserQuery(command.Email, command.Password);
        var dataModel = await userManager.GetAsync(userQuery);

        if (dataModel is null)
            return responseFactory.Create();

        var result = await signInManager.PasswordSignInAsync(dataModel, command.Password, false, false);

        if (result.Succeeded)
            token = tokenFactory.GenerateToken(dataModel);

        return responseFactory.Create(result, token);
    }
}
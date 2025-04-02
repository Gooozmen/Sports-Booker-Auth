using Application.Interfaces;
using MediatR;
using Shared.Commands;
using Shared.Enums;
using Shared.Responses;
using Shared.Queries;

namespace Application.Handlers;

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
        var userQuery = new UserQuery(command.Email, (int)IdentityPropertyTypes.UserEmail);
        var dataModel = await userManager.GetAsync(userQuery);

        if (dataModel is null)
            return responseFactory.Create();

        var result = await signInManager.PasswordSignInAsync(dataModel, command.Password, false, false);

        if (result.Succeeded)
            token = tokenFactory.Create(dataModel);

        var response = responseFactory.Create(result, token);
        return response;
    }
}
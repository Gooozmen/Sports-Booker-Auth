using Application.Interfaces;
using MediatR;
using Shared.Commands;
using Shared.Enums;
using Shared.Responses;
using Shared.Queries;
using Shared.Responses.Auth;

namespace Application.Handlers;

public class LoginCommandHandler(
    ILoginManager loginManager,
    IApplicationUserManager userManager,
    ITokenFactory tokenFactory
)
    : IRequestHandler<LoginCommand, Result>
{
    public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var dataModel = await userManager.GetAsync(new UserQuery(command.Email, (int)IdentityPropertyTypes.UserEmail));

        if (dataModel is null)
            return new AuthFailedResponse("Login Failed - Username not found.",false);

        var result = await loginManager.CheckPasswordAsync(dataModel, command.Password);

        if (result)
           return new LoginResponse(tokenFactory.Create(dataModel),true);
        
        return new AuthFailedResponse("Authentication Failed - Invalid username or password.",false);
    }
}
using CourtBooker.Auth.Application.Interfaces;
using MediatR;
using CourtBooker.Auth.Shared.Commands;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Queries;
using CourtBooker.Auth.Shared.Responses;

namespace CourtBooker.Auth.Application.Handlers;

public class LoginCommandHandler(
    ILoginManager loginManager,
    IApplicationUserManager userManager,
    ITokenFactory tokenFactory
)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var dataModel = await userManager.GetAsync(new UserQuery(command.Email, (int)IdentityPropertyTypes.UserEmail));

        if (dataModel is null)
            return new LoginResponse().Failed("Login Failed - Username not found.");

        var result = await loginManager.CheckPasswordAsync(dataModel, command.Password);

        if (result)
            return new LoginResponse().Success(tokenFactory.Create(dataModel));

        return new LoginResponse().Failed("Authentication Failed - Invalid username or password.");
    }
}
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationUser;

namespace Application.CommandHandlers;

public class SignInCommandHandler : ISignInCommandHandler
{
    
}

public interface ISignInCommandHandler
{
    // Task<IdentityResult> ExecuteUserLoginAsync(LoginUserCommand command);
}
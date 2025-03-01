using Castle.Components.DictionaryAdapter.Xml;
using Infrastructure.Factories;
using Infrastructure.IdentityManagers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationUser;

namespace Application.CommandHandlers;

public class SignInCommandHandler
(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    ITokenFactory tokenFactory
) 
    : ISignInCommandHandler
{
    public async Task<SignInResult> ExecutePasswordSignInAsync(PasswordSignInCommand command)
    {
        var dataModel = await userManager.FindByEmailAsync(command.Email);
        
        if (dataModel is null) return SignInResult.NotAllowed;
        
        var result = await signInManager.PasswordSignInAsync(dataModel, command.Password, false, false);
        
        // if(result.Succeeded) tokenFactory.GenerateToken(dataModel);
        
        return result;
    }
}

public interface ISignInCommandHandler
{
    Task<SignInResult> ExecutePasswordSignInAsync(PasswordSignInCommand command);
}
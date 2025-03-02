using Application.Builders;
using Application.Decorators;
using Castle.Components.DictionaryAdapter.Xml;
using Infrastructure.Factories;
using Infrastructure.IdentityManagers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationUser;
using Shared.Responses;
using Shared.Wrappers;

namespace Application.CommandHandlers;

public class SignInCommandHandler
(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    ISignInResultDecorator signInDecorator,
    ITokenFactory tokenFactory
) 
    : ISignInCommandHandler
{
    public async Task<SignInWrapper> ExecutePasswordSignInAsync(PasswordSignInCommand command)
    {
        string token = string.Empty;
        
        var dataModel = await userManager.FindByEmailAsync(command.Email);

        if (dataModel is null) 
            return signInDecorator.NotAllowed();
        
        var result = await signInManager.PasswordSignInAsync(dataModel, command.Password, false, false);
        
        if(result.Succeeded) 
            token = tokenFactory.GenerateToken(dataModel);
        
        return signInDecorator.Success(token);
    }
}

public interface ISignInCommandHandler
{
    Task<SignInWrapper> ExecutePasswordSignInAsync(PasswordSignInCommand command);
}
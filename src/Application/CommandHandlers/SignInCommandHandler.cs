using Application.Decorators;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Shared.Commands;
using Shared.Commands.ApplicationUser;
using Shared.Interfaces;
using Shared.Wrappers;

namespace Application.CommandHandlers;

public class SignInCommandHandler
(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    ISignInResultDecorator signInDecorator,
    ITokenFactory tokenFactory
) 
    : ICommandHandler<ICommand, Object>
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

    public async Task<Object> Handle(ICommand command)
    {
        return command switch
        {
            PasswordSignInCommand cmd => await ExecutePasswordSignInAsync(cmd),
            _ => new NotDefinedCommand()
        };
    }

}
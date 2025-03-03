using Application.Decorators;
using Application.Interfaces;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Commands;
using Shared.Responses;

namespace Application.CommandHandlers;

public class PasswordSignInCommandHandler
(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    ISignInResultDecorator signInDecorator,
    ITokenFactory tokenFactory
) 
    : IRequestHandler<PasswordSignInCommand,PasswordSignInResponse>
{
    
    public async Task<PasswordSignInResponse> Handle(PasswordSignInCommand command, CancellationToken cancellationToken)
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


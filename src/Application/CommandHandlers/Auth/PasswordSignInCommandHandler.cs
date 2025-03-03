using Application.Factories;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Responses;

namespace Application.CommandHandlers;

public class PasswordSignInCommandHandler
(
    IApplicationSignInManager signInManager,
    IApplicationUserManager userManager,
    IPasswordSignInResponseFactory responseFactory,
    ITokenFactory tokenFactory
) 
    : IRequestHandler<PasswordSignInCommand,SignInResponseBase>
{
    
    public async Task<SignInResponseBase> Handle(PasswordSignInCommand command, CancellationToken cancellationToken)
    {
        string token = string.Empty;
        SignInResult result = new SignInResult();
        
        var dataModel = await userManager.FindByEmailAsync(command.Email);

        if (dataModel is null)
            return responseFactory.Create();
        
        result = await signInManager.PasswordSignInAsync(dataModel, command.Password, false, false);
        
        if(result.Succeeded) 
            token = tokenFactory.GenerateToken(dataModel);
        
        return responseFactory.Create(result, token);
    }
}


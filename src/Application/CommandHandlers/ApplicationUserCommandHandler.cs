using Application.Builders;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Commands.ApplicationUser;

namespace Application.CommandHandlers;

public class ApplicationUserCommandHandler : IApplicationUserCommandHandler
{
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IApplicationUserBuilder _userBuilder;
    
    public ApplicationUserCommandHandler
    (
        IApplicationUserManager applicationUserManager,
        IApplicationUserBuilder userBuilder
    )
    {
        _applicationUserManager = applicationUserManager;
        _userBuilder = userBuilder;
    }
    public async Task<IdentityResult> ExecuteCreateAsync(CreateUserCommand cmd)
    {
        var dataModel = _userBuilder.Apply(cmd);
        var result = await _applicationUserManager.CreateUserAsync(dataModel,cmd.Password);
        return result;
    } 
}

public interface IApplicationUserCommandHandler 
{
    Task<IdentityResult> ExecuteCreateAsync( CreateUserCommand cmd);
}
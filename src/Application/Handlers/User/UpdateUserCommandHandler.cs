using Application.Builders;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Enums;
using Shared.Queries;
using Shared.Wrappers;

namespace Application.Handlers;

public class UpdateUserCommandHandle(
    IApplicationUserManager userManager,
    IApplicationUserBuilder userBuilder)
    : IRequestHandler<UpdateUserCommand,IdentityResult> 
{
    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dataModel = await userManager.GetAsync
        (
            new UserQuery(request.Id.ToString(), 
            (int)IdentityPropertyTypes.UserId)
        );
        
        var result = await userManager.UpdateAsync(new ApplicationUserWrapper{ ApplicationUser = dataModel});

        return result;
    }
}
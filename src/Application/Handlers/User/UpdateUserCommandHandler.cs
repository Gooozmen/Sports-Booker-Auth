using Application.Builders;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Commands;
using Shared.Enums;
using Shared.Queries;
using Shared.Wrappers;

namespace Application.Handlers;

public class UpdateUserCommandHandler
    (IApplicationUserManager userManager) 
    : IRequestHandler<UpdateUserCommand,IdentityResult> 
{
    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken = default)
    {
        var dataModel = await userManager.GetAsync(
            new UserQuery(request.Id.ToString(), (int)IdentityPropertyTypes.UserId)
        );

        if (dataModel == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var result = await userManager.UpdateAsync(new ApplicationUserWrapper
        {
            ApplicationUser = new ApplicationUser
            {
                Id = dataModel.Id,
                UserName = string.IsNullOrEmpty(request.Username) ? dataModel.UserName : request.Username,
                Email = string.IsNullOrEmpty(request.Email) ? dataModel.Email : request.Email,
                PasswordHash = string.IsNullOrEmpty(request.Password) ? dataModel.PasswordHash : request.Password,
            }
        });

        return result;
    }
}
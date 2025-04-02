using Application.Interfaces;
using Domain.Models;
using Shared.Commands;
using Shared.Responses.User;

namespace Application.Builders;

public class ApplicationUserBuilder : IApplicationUserBuilder
{
    public ApplicationUser Apply(CreateUserCommand cmd)
        =>  new ApplicationUser
        {
            Email = cmd.Email,
            UserName = cmd.Email,
            PhoneNumber = string.IsNullOrEmpty(cmd.PhoneNumber) ? null : cmd.PhoneNumber,
            Active = true
        };

    public UserResponse Apply(ApplicationUser model)
        =>  new UserResponse
        {
            Id = model.Id,
            Username = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Active = model.Active,
            EmailConfirmed = model.EmailConfirmed,
            PhoneNumberConfirmed = model.PhoneNumberConfirmed,
            TwoFactorEnabled = model.TwoFactorEnabled,
            LockoutEnabled = model.LockoutEnabled, 
            IsSuccess = true
        };


    public ApplicationUser Apply(UpdateUserCommand command)
        => new ApplicationUser
        {
            Id = command.Id,
            Email = command.Email,
            UserName = command.Email,
            PhoneNumber = command.PhoneNumber
        };
} 

public interface IApplicationUserBuilder 
    : IBuilder<CreateUserCommand, ApplicationUser>, 
      IBuilder<ApplicationUser, UserResponse>,
      IBuilder<UpdateUserCommand, ApplicationUser>
{
}
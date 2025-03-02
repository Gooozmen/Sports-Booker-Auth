using Application.Interfaces;
using Domain.Models;
using Shared.Commands.ApplicationUser;

namespace Application.Builders;

public class ApplicationUserBuilder : IBuilder<CreateUserCommand, ApplicationUser>
{
    public ApplicationUser Apply(CreateUserCommand cmd)
    {
        return new ApplicationUser
        {
            Email = cmd.Email,
            UserName = cmd.Email,
            PhoneNumber = string.IsNullOrEmpty(cmd.PhoneNumber) ? null : cmd.PhoneNumber,
            Active = true
        };
    }
}
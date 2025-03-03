using Application.Interfaces;
using Domain.Models;
using Shared.Commands;

namespace Application.Builders;

public class ApplicationUserBuilder : IApplicationUserBuilder
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

public interface IApplicationUserBuilder : IBuilder<CreateUserCommand, ApplicationUser>
{
}
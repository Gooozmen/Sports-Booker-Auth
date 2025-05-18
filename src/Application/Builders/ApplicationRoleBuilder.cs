using CourtBooker.Auth.Application.Interfaces;
using CourtBooker.Auth.Domain.Models;
using CourtBooker.Auth.Shared.Commands;

namespace CourtBooker.Auth.Application.Builders;

public class ApplicationRoleBuilder : IApplicationRoleBuilder
{
    public ApplicationRole Apply(CreateRoleCommand cmd)
    {
        return new ApplicationRole
        {
            Name = cmd.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Active = true
        };
    }
}

public interface IApplicationRoleBuilder : IBuilder<CreateRoleCommand, ApplicationRole>
{
}
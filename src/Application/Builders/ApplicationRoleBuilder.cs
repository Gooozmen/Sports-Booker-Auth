using Application.Interfaces;
using Domain.Models;
using Shared.Commands;

namespace Application.Builders;

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
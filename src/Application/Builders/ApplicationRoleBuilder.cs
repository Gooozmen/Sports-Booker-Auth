using Domain.Models;
using Shared.Commands.ApplicationRole;

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

public interface IApplicationRoleBuilder
{
    ApplicationRole Apply(CreateRoleCommand cmd);
}
using Application.Interfaces;
using Domain.Models;
using Shared.Commands;

namespace Application.Builders;


public class ApplicationRoleBuilder : IBuilder<CreateRoleCommand, ApplicationRole>
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
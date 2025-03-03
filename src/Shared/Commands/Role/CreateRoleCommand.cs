using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Interfaces;
using Shared.Responses.Role;

namespace Shared.Commands;

public sealed record CreateRoleCommand : IRequest<CreateRoleResponse>, ICommand
{
    [Length(3,10)]
    public required string Name { get; set; }
}
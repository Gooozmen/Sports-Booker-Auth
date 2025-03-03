using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Commands.ApplicationRole;

public sealed record CreateRoleCommand : ICommand
{
    [Required]
    [Length(3,10)]
    public string Name { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Shared.Commands.ApplicationRole;

public class CreateRoleCommand
{
    [Required]
    [Length(3,10)]
    public string Name { get; set; }
}
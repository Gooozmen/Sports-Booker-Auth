using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Commands;

public sealed record CreateUserCommand : ICommand
{
    [EmailAddress]
    public required string Email { get; set; }
    [Length(8,16)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).{8,16}$", ErrorMessage = "Password must be 8-16 characters, contain at least one uppercase letter and one special character.")]
    public required string Password { get; set; }
    public string? PhoneNumber { get; set; }

}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Shared.Interfaces;
using Shared.Responses;

namespace Shared.Commands;

public sealed record PasswordSignInCommand : IRequest<SignInResponseBase>, ICommand
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [PasswordPropertyText] public required string Password { get; set; }
}
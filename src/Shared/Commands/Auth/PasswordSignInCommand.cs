using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;
using MediatR;
using Shared.Responses;

namespace Shared.Commands;

public sealed record PasswordSignInCommand : IRequest<SignInResponseBase>, ICommand
{
    [EmailAddress]
    public required string Email { get; set; }
    [PasswordPropertyText]
    public required string Password { get; set; }
}
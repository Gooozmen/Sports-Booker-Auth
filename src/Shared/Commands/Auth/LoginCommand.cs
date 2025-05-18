using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using Shared.Interfaces;
using Shared.Responses;
using Shared.Responses.Auth;

namespace Shared.Commands;

public sealed record LoginCommand : IRequest<LoginResponse>, ICommand
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [PasswordPropertyText] public required string Password { get; set; }
    
}
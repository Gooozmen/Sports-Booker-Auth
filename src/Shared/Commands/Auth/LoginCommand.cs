using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CourtBooker.Auth.Shared.Interfaces;
using CourtBooker.Auth.Shared.Responses;
using MediatR;

namespace CourtBooker.Auth.Shared.Commands;

public sealed record LoginCommand : IRequest<LoginResponse>, ICommand
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [PasswordPropertyText] public required string Password { get; set; }
}
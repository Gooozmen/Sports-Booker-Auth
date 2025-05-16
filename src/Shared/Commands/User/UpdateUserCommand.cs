using System.Text.Json.Serialization;
using CourtBooker.Auth.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Shared.Commands;

public sealed record UpdateUserCommand : IRequest<IdentityResult>, ICommand
{
    [JsonIgnore] public Guid Id { get; set; }

    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? Username { get; init; }
    public string? PhoneNumber { get; init; }
}
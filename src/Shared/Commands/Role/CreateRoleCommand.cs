using System.ComponentModel.DataAnnotations;
using CourtBooker.Auth.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Shared.Commands;

public sealed record CreateRoleCommand : IRequest<IdentityResult>, ICommand
{
    [Required] [Length(3, 10)] public required string Name { get; set; }
}
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;

namespace Shared.Commands;

public sealed record CreateRoleCommand : IRequest<IdentityResult>, ICommand
{
    [Required] [Length(3, 10)] public required string Name { get; set; }
}
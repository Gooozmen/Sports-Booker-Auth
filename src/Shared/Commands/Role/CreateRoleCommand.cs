using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using Shared.Responses;

namespace Shared.Commands;

public sealed record CreateRoleCommand : IRequest<IdentityResult>, ICommand
{
    [Length(3,10)]
    public required string Name { get; set; }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using Shared.Interfaces;
using Shared.Responses;

namespace Shared.Commands;

public sealed record LogoutCommand: IRequest<IResponse>, ICommand, IPropertyType
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [PasswordPropertyText] public required string Password { get; set; }

    [JsonIgnore]
    public int PropertyType { get; set; }
}
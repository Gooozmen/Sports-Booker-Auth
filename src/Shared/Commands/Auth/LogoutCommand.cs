using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CourtBooker.Auth.Shared.Interfaces;
using MediatR;
using CourtBooker.Auth.Shared.Responses;

namespace CourtBooker.Auth.Shared.Commands;

public sealed record LogoutCommand: IRequest<IResponse>, ICommand, IPropertyType
{
    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [PasswordPropertyText] public required string Password { get; set; }

    [JsonIgnore]
    public int PropertyType { get; set; }
}
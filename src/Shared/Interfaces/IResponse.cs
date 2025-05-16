using System.Text.Json.Serialization;

namespace CourtBooker.Auth.Shared.Interfaces;

public interface IResponse
{
    [JsonIgnore] bool IsSuccess { get; set; }
}
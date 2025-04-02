using System.Text.Json.Serialization;

namespace Shared.Interfaces;

public interface IResponse
{
    [JsonIgnore]
    bool IsSuccess { get; set; }
}
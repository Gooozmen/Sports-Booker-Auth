using System.Text.Json.Serialization;

namespace Shared.Responses;

public abstract class Result(bool success)
{
    [JsonIgnore]
    public bool Success { get; } = success;
}
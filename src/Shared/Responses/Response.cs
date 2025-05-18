using CourtBooker.Auth.Shared.Interfaces;

namespace CourtBooker.Auth.Shared.Responses;

public class Response<T> : IResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}
using Shared.Interfaces;

namespace Shared.Responses;

public class ControllerResponse<T> : IResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
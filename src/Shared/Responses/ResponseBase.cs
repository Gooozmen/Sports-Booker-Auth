using Shared.Enums;

namespace Shared.Responses;

public class ResponseBase<T>
{

    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
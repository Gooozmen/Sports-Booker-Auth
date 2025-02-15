using System.Net;
using Shared.Enums;
using Shared.Responses;

namespace Application.Builders;

public class HttpResponseBuilder : IHttpResponseBuilder
{

    public ResponseBase<T> CreateResponse<T>(int statusCode,T data, string message = "")
    {
        return new ResponseBase<T>
        {
            IsSuccess = SetSuccess(statusCode),
            Data = data,
            Message = string.IsNullOrEmpty(message) ? HttpStatusDescriptions.GetDescription(statusCode) : message,
            StatusCode = statusCode
        };
    }

    private bool SetSuccess(int httpStatusCode) =>
                httpStatusCode switch
                {
                    >= 200 and < 300 => true, // Success range
                    _ => false               // Failure range
                };
}

public interface IHttpResponseBuilder
{
    ResponseBase<T> CreateResponse<T>(int statusCode, T data, string message = "");
}
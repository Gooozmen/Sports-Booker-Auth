using System.Net;
using Shared.Enums;
using Shared.Responses;

namespace Application.Builders;

public class HttpResponseBuilder : IHttpResponseBuilder
{

    public ControllerResponse<T> CreateResponse<T>(int statusCode,T data, string message = "")
    {
        return new ControllerResponse<T>
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
    ControllerResponse<T> CreateResponse<T>(int statusCode, T data, string message = "");
}
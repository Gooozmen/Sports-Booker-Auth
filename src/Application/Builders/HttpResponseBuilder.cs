using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Responses;

namespace CourtBooker.Auth.Application.Builders;

public class HttpResponseBuilder : IHttpResponseBuilder
{
    public Response<T> CreateResponse<T>(int statusCode, T data, string message)
    {
        return new Response<T>
        {
            IsSuccess = SetSuccess(statusCode),
            Data = data,
            Message = string.IsNullOrEmpty(message) ? HttpStatusDescriptions.GetDescription(statusCode) : message,
            StatusCode = statusCode
        };
    }

    public Response<T> CreateResponse<T>(int statusCode, T data)
    {
        return CreateResponse(statusCode, data, "");
    }


    private bool SetSuccess(int httpStatusCode)
    {
        return httpStatusCode switch
        {
            >= 200 and < 300 => true, // Success range
            _ => false // Failure range
        };
    }
}

public interface IHttpResponseBuilder
{
    Response<T> CreateResponse<T>(int statusCode, T data, string message);
    Response<T> CreateResponse<T>(int statusCode, T data);
}
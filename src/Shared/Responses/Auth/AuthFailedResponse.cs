namespace Shared.Responses.Auth;

public class AuthFailedResponse(string message,bool success) : Result(success)
{
    public string Message { get; set; } = message;
}
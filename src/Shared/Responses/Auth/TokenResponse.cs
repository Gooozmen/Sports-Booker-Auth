namespace Shared.Responses.Auth;

public class TokenResponse(string bearer,bool success) : Result(success)
{
    public string Bearer { get; set; } = bearer;
}
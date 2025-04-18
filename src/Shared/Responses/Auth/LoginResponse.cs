using Shared.Responses.User;

namespace Shared.Responses.Auth;

public class LoginResponse(string bearer,bool success) : Result(success)
{
    public string Bearer { get; set; } = bearer;
}
using Microsoft.AspNetCore.Authorization;
using Shared.Interfaces;
using Shared.Wrappers;

namespace Shared.Responses.Auth;

public class LoginResponse: IResponse
{
    public AccessToken? AccessToken { get; set; }
    public string? Error { get; set; }
    public bool IsSuccess { get; set; }
    
    public LoginResponse Failed(string errorMessage)
        => new LoginResponse { Error = errorMessage, IsSuccess = false };
    
    public LoginResponse Success(string token)
        => new LoginResponse { AccessToken = new AccessToken(token), IsSuccess = true };
    
}

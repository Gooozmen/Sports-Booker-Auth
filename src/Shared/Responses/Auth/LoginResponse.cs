using CourtBooker.Auth.Shared.Interfaces;
using CourtBooker.Auth.Shared.Wrappers;

namespace CourtBooker.Auth.Shared.Responses;

public class LoginResponse : IResponse
{
    public AccessToken? AccessToken { get; set; }
    public string? Error { get; set; }
    public bool IsSuccess { get; set; }

    public LoginResponse Failed(string errorMessage)
    {
        return new LoginResponse { Error = errorMessage, IsSuccess = false };
    }

    public LoginResponse Success(string token)
    {
        return new LoginResponse { AccessToken = new AccessToken(token), IsSuccess = true };
    }
}
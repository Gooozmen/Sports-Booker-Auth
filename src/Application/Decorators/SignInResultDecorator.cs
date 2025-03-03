using Shared.Responses;

namespace Application.Decorators;

public class SignInResultDecorator : ISignInResultDecorator
{
    public PasswordSignInResponse Success(string data) =>
        new PasswordSignInResponse(true, false, false, false, data);

    public PasswordSignInResponse LockedOut() => 
        new PasswordSignInResponse(false, true, false, false, "User is locked out");

    public PasswordSignInResponse NotAllowed() =>
        new PasswordSignInResponse(false, false, true, false, "User is not allowed to sign in");

    public PasswordSignInResponse TwoFactorRequired() =>
        new PasswordSignInResponse(false, false, false, true, "Two-factor authentication is required");

    public PasswordSignInResponse Failed(string data = "Invalid credentials") =>
        new PasswordSignInResponse(false, false, false, false, data);
}

public interface ISignInResultDecorator
{
    PasswordSignInResponse Success(string data);
    PasswordSignInResponse LockedOut();
    PasswordSignInResponse NotAllowed();
    PasswordSignInResponse TwoFactorRequired();
    PasswordSignInResponse Failed(string data = "Invalid credentials");
}

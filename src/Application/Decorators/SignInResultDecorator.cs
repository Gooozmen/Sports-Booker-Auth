using Shared.Wrappers;

namespace Application.Decorators;

public class SignInResultDecorator : ISignInResultDecorator
{
    public SignInWrapper Success(string data) =>
        new SignInWrapper(true, false, false, false, data);

    public SignInWrapper LockedOut() => 
        new SignInWrapper(false, true, false, false, "User is locked out");

    public SignInWrapper NotAllowed() =>
        new SignInWrapper(false, false, true, false, "User is not allowed to sign in");

    public SignInWrapper TwoFactorRequired() =>
        new SignInWrapper(false, false, false, true, "Two-factor authentication is required");

    public SignInWrapper Failed(string data = "Invalid credentials") =>
        new SignInWrapper(false, false, false, false, data);
}

public interface ISignInResultDecorator
{
    SignInWrapper Success(string data);
    SignInWrapper LockedOut();
    SignInWrapper NotAllowed();
    SignInWrapper TwoFactorRequired();
    SignInWrapper Failed(string data = "Invalid credentials");
}

namespace Shared.Wrappers;

public class SignInWrapper
{
    public SignInWrapper(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor, string data)
    {
        Succeeded = succeeded;
        IsLockedOut = isLockedOut;
        IsNotAllowed = isNotAllowed;
        RequiresTwoFactor = requiresTwoFactor;
        Data = data;
    }

    public bool Succeeded { get; set; }
    public bool IsLockedOut { get; set; }
    public bool IsNotAllowed { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public string Data { get; set; } // Custom Message
}
namespace Shared.Responses;

public abstract class BaseIdentityResponse
{

    protected BaseIdentityResponse(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor, string data)
    {
        Succeeded = succeeded;
        IsLockedOut = isLockedOut;
        IsNotAllowed = isNotAllowed;
        RequiresTwoFactor = requiresTwoFactor;
        Data = data;
    }

    public bool Succeeded { get; protected set; }
    public bool IsLockedOut { get; protected set; }
    public bool IsNotAllowed { get; protected set; }
    public bool RequiresTwoFactor { get; protected set; }
    public string Data { get; protected set; } // Custom Message
}
using Microsoft.AspNetCore.Identity;

namespace Shared.Responses;

public class SignInFailed : SignInResponseBase
{
    public SignInFailed(bool isLockedOut, bool requiresTwoFactor, bool succeeded, bool notAllowed) 
        : base(isLockedOut, requiresTwoFactor, succeeded, notAllowed)
    { }
    public SignInFailed(SignInResult result) : base(result)
    { }
}
public class SignInSuccess(SignInResult result, string token) : SignInResponseBase(result)
{ }

public abstract class SignInResponseBase(bool isLockedOut, bool requiresTwoFactor, bool succeded, bool notAllowed)
{
    protected bool IsLockedOut { get; set; } = isLockedOut;
    protected bool RequiresTwoFactor { get; set; } = requiresTwoFactor;
    protected bool Succeded { get; set; } = succeded;
    protected bool NotAllowed { get; set; } = notAllowed;
    protected SignInResponseBase(SignInResult result) : this(result.IsLockedOut, result.RequiresTwoFactor, result.Succeeded, result.IsNotAllowed)
    { }
}
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;

namespace Shared.Responses;

public class SignInFailed(SignInResult result) : SignInResponseBase(result);

public class SignInSuccess(SignInResult result, string token) : SignInResponseBase(result)
{
    public string Token {get;set;} = token;
}

public abstract class SignInResponseBase(bool isLockedOut, bool requiresTwoFactor, bool succeeded, bool notAllowed) : IResponse
{
    protected SignInResponseBase(SignInResult result) 
        : this(result.IsLockedOut, result.RequiresTwoFactor, result.Succeeded, result.IsNotAllowed) 
    { }

    protected bool IsLockedOut { get; set; } = isLockedOut;
    protected bool RequiresTwoFactor { get; set; } = requiresTwoFactor;
    protected bool Succeeded { get; set; } = succeeded;
    protected bool NotAllowed { get; set; } = notAllowed;
    [JsonIgnore]
    public bool IsSuccess { get; set; } = succeeded;
}
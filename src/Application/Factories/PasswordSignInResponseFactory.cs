using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Responses;

namespace Application.Factories;

public class PasswordSignInResponseFactory : IPasswordSignInResponseFactory
{
    public SignInResponseBase Create(SignInResult? source, string token)
    {
        return source switch
        {
            { Succeeded: true } => new SignInSuccess(source, token),
            _ => ValidateFailure(source)
        };
    }

    public SignInResponseBase Create()
    {
        return Create(null, string.Empty);
    }

    private SignInResponseBase ValidateFailure(SignInResult? source)
    {
        return new SignInFailed(source);
    }
}
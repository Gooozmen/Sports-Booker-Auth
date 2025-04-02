using Microsoft.AspNetCore.Identity;
using Shared.Responses;

namespace Application.Interfaces;

public interface IPasswordSignInResponseFactory : IFactory<SignInResult, SignInResponseBase, string>
{
}
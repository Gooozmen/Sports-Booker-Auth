using System.Security.Claims;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityManagers;

public class ApplicationSignInManager
    (SignInManager<ApplicationUser> signInManager) 
    : IApplicationSignInManager
{
    public bool IsSignInAsync(ClaimsPrincipal user)
    {
        var result = signInManager.IsSignedIn(user);
        return result;
    }

    public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        var result = await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        return result;
    }

    public async Task SignInAsync(ApplicationUser user, bool isPersistent)
    {
        await signInManager.SignInAsync(user, isPersistent);
    }
}

public interface IApplicationSignInManager
{
    Task SignInAsync(ApplicationUser user, bool isPersistent);

    Task<SignInResult> PasswordSignInAsync(ApplicationUser user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure);

    bool IsSignInAsync(ClaimsPrincipal user);
}
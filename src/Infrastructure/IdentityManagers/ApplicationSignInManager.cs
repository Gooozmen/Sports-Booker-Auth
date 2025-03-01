using System.Security.Claims;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityManagers;

public class ApplicationSignInManager : IApplicationSignInManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    public ApplicationSignInManager(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public bool IsSignInAsync(ClaimsPrincipal user)
    {
        var result = _signInManager.IsSignedIn(user);
        return result;
    }

    public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, 
                                                        string password, 
                                                        bool isPersistent,
                                                        bool lockoutOnFailure)
    {
        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        return result;
    }

    public async Task SignInAsync(ApplicationUser user, bool isPersistent)
    {
        await _signInManager.SignInAsync(user, isPersistent);
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
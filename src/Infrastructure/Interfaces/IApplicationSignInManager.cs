using System.Security.Claims;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IApplicationSignInManager
{
    Task SignInAsync(ApplicationUser user, bool isPersistent);

    Task<SignInResult> PasswordSignInAsync(ApplicationUser user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure);

    bool IsSignInAsync(ClaimsPrincipal user);
}
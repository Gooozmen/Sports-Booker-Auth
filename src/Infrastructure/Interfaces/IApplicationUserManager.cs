using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IApplicationUserManager
{
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
}
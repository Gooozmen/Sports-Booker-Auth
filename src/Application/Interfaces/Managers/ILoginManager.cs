using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface ILoginManager
{
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
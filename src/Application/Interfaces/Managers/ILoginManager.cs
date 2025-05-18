using CourtBooker.Auth.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CourtBooker.Auth.Application.Interfaces;

public interface ILoginManager
{
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
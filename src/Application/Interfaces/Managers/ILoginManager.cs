using CourtBooker.Auth.Domain.Models;

namespace CourtBooker.Auth.Application.Interfaces;

public interface ILoginManager
{
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
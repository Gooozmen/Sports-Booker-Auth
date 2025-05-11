using Domain.Models;

namespace Application.Interfaces;

public interface ILoginManager
{
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
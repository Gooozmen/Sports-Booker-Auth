using Domain.Models;

namespace Infrastructure.Interfaces;

public interface ITokenFactory
{
    string GenerateToken(ApplicationUser user);
}
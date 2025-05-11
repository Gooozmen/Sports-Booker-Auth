using Domain.Models;

namespace Application.Interfaces;

public interface ITokenFactory : IFactory<ApplicationUser, string>
{
}
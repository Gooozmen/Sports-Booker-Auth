using CourtBooker.Auth.Domain.Models;

namespace CourtBooker.Auth.Application.Interfaces;

public interface ITokenFactory : IFactory<ApplicationUser, string>
{
}
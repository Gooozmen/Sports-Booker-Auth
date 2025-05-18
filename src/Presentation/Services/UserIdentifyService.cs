using System.Security.Claims;

namespace CourtBooker.Auth.Presentation.Services;

public class UserIdentifyService(IHttpContextAccessor httpContextAccessor) : IUserIdentifyService
{
    public string? GetUserId() => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}

public interface IUserIdentifyService
{
    string? GetUserId();
}
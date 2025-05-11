using System.Security.Claims;

namespace Presentation.Services;

public class UserIdentifyService(IHttpContextAccessor httpContextAccessor) : IUserIdentifyService
{
    public string? GetUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

public interface IUserIdentifyService
{
    string? GetUserId();
}
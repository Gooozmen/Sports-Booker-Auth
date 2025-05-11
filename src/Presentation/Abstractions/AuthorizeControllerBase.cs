using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

[Authorize]
public abstract class AuthorizeControllerBase(IServiceProvider serviceProvider)
    : ControllerBase(serviceProvider)
{
}
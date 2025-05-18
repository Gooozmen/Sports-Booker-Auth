using Microsoft.AspNetCore.Authorization;

namespace CourtBooker.Auth.Presentation.Controllers;

[Authorize]
public abstract class AuthorizeControllerBase(IServiceProvider serviceProvider)
    : ControllerBase(serviceProvider)
{ } 
using CourtBooker.Auth.Application.Builders;
using CourtBooker.Auth.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Auth.Presentation.Controllers;

[Controller]
[Route("api/[controller]/")]
public abstract class ControllerBase(IServiceProvider serviceProvider) : Controller
{
    protected IHttpResponseBuilder ResponseBuilder { get;} = serviceProvider.GetRequiredService<IHttpResponseBuilder>();
    protected IUserIdentifyService UserIdentifyService { get;} = serviceProvider.GetRequiredService<IUserIdentifyService>();
}
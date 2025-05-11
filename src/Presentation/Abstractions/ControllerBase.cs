using Application.Builders;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

namespace Presentation.Controllers;

[Controller]
[Route("api/[controller]/")]
public abstract class ControllerBase(IServiceProvider serviceProvider) : Controller
{
    protected IHttpResponseBuilder ResponseBuilder { get; } =
        serviceProvider.GetRequiredService<IHttpResponseBuilder>();

    protected IUserIdentifyService UserIdentifyService { get; } =
        serviceProvider.GetRequiredService<IUserIdentifyService>();
}
using Application.Builders;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

namespace Presentation.Controllers;

[Controller]
[Route("api/[controller]")]
public abstract class ControllerBase : Controller
{
    protected IHttpResponseBuilder ResponseBuilder { get;}
    protected IUserIdentifyService UserIdentifyService { get;}
    
    private readonly IServiceProvider _serviceProvider;
    protected ControllerBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        ResponseBuilder = _serviceProvider.GetRequiredService<IHttpResponseBuilder>();
        UserIdentifyService = _serviceProvider.GetRequiredService<IUserIdentifyService>();
    }

}
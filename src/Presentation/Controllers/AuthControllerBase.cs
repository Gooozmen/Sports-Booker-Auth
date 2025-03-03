using Application.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Controller]
// [Authorize]
[Route("api/[controller]")]
public abstract class AuthControllerBase(IHttpResponseBuilder responseBuilder) : Controller
{
    protected readonly IHttpResponseBuilder ResponseBuilder = responseBuilder;
}
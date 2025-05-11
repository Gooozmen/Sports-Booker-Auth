using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AuthController(
    ISender sender,
    IServiceProvider serviceProvider
) : ControllerBase(serviceProvider)
{
    [HttpPost("login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {
            { IsSuccess: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result.AccessToken)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result.Error))
        };
    }

    [HttpPost("logout")]
    public async Task<IActionResult> ProcessUserLogoutAsync([FromBody] LogoutCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {
            { IsSuccess: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result))
        };
    }

    [HttpPost("register")]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {
            { Succeeded: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.Created, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result.Errors))
        };
    }
}
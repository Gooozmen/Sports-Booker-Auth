using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.Responses;
using Shared.Responses.Auth;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AuthController(
    ISender sender,
    IServiceProvider serviceProvider
) : ControllerBase(serviceProvider)
{

    [HttpPost("Login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {  { Success: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result.As<TokenResponse>())),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result.As<AuthFailedResponse>()))
        };
    }
}
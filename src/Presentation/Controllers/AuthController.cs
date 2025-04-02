using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.Responses;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AuthController(
    ISender sender,
    IServiceProvider serviceProvide
) : ControllerBase(serviceProvide)
{

    [HttpPost("Login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] PasswordSignInCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {
            SignInSuccess => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result.As<SignInSuccess>().Token)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result.As<SignInFailed>()))
        };

    }
}
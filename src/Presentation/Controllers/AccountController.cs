using System.Net;
using Application.Builders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.Responses;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AccountController(
    ISender sender,
    IHttpResponseBuilder responseBuilder
) : AuthControllerBase(responseBuilder)
{
    [HttpPost("Register")]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var result = await sender.Send(command);

        return result switch
        {
            { Succeeded: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.Created, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result))
        };
    }

    [HttpPost("Login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] PasswordSignInCommand command)
    {
        var result = await sender.Send(command);

        return result switch
        {
            SignInSuccess => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result))
        };
    }
}
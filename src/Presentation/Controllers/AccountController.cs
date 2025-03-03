using System.Net;
using Application.Builders;
using Application.CommandHandlers;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands.ApplicationUser;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AccountController 
    (
        IHttpResponseBuilder responseBuilder
    ) : AuthControllerBase(responseBuilder)
{
    
    [HttpPost("Register")]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var identityResult = await  .ExecuteCreateAsync(command);

        return identityResult.Succeeded switch
        {
            true => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.Created, identityResult)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, identityResult.Errors))
        };
    }

    [HttpPost("Login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] PasswordSignInCommand command)
    {
        var result = await _signInCommandHandler.ExecutePasswordSignInAsync(command);

        return result.Succeeded switch
        {
            true => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result)),
            false => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.Unauthorized, result)),
        };
    }
}
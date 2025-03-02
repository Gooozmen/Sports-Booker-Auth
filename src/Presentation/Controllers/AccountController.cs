using System.Net;
using Application.Builders;
using Application.CommandHandlers;
using Domain.Models;
using Infrastructure.IdentityManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands.ApplicationUser;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AccountController : AuthControllerBase
{
    private readonly IApplicationUserCommandHandler _applicationUserCommandHandler;
    private readonly ISignInCommandHandler _signInCommandHandler;
    private readonly IHttpResponseBuilder _responseBuilder;

    public AccountController
    (
        IApplicationUserCommandHandler userCommandHandler, 
        ISignInCommandHandler signInCommandHandler,
        IHttpResponseBuilder responseBuilder
    )
    {
        _applicationUserCommandHandler = userCommandHandler;
        _signInCommandHandler = signInCommandHandler;
        _responseBuilder = responseBuilder;
        
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var identityResult = await _applicationUserCommandHandler.ExecuteCreateAsync(command);

        return identityResult.Succeeded switch
        {
            true => Ok(_responseBuilder.CreateResponse((int)HttpStatusCode.Created, identityResult)),
            _ => BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, identityResult.Errors))
        };
    }

    [HttpPost("Login")]
    public async Task<IActionResult> ProcessUserLoginAsync([FromBody] PasswordSignInCommand command)
    {
        var result = await _signInCommandHandler.ExecutePasswordSignInAsync(command);

        if (result.IsNotAllowed)
            return BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result));
        
        else if (result.Succeeded)
            return Ok(_responseBuilder.CreateResponse((int)HttpStatusCode.OK, result));

        else
            return StatusCode(500, "Internal Server Error");
            
        
    }
}
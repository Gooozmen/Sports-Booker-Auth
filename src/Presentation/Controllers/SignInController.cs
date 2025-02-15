using System.Net;
using Application.Builders;
using Application.CommandHandlers;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands.ApplicationUser;

namespace Presentation.Controllers;

public class SignInController : AuthControllerBase
{
    private readonly IApplicationUserCommandHandler _applicationUserCommandHandler;
    private readonly IHttpResponseBuilder _responseBuilder;

    public SignInController
    (
        IApplicationUserCommandHandler userCommandHandler, 
        IHttpResponseBuilder responseBuilder
    )
    {
        _applicationUserCommandHandler = userCommandHandler;
        _responseBuilder = responseBuilder;
        
    }

    
    [HttpPost("signin")]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var identityResult = await _applicationUserCommandHandler.ExecuteCreateAsync(command);

        return identityResult.Succeeded switch
        {
            true => Ok(_responseBuilder.CreateResponse((int)HttpStatusCode.OK, identityResult)),
            _ => BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, identityResult.Errors))
        };
    }

}
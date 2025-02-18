using System.Net;
using Application.Builders;
using Application.CommandHandlers;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands.ApplicationUser;

namespace Presentation.Controllers;

public class SigninController : AuthControllerBase
{
    private readonly IApplicationUserCommandHandler _applicationUserCommandHandler;
    private readonly IHttpResponseBuilder _responseBuilder;

    public SigninController
    (
        IApplicationUserCommandHandler userCommandHandler, 
        IHttpResponseBuilder responseBuilder
    )
    {
        _applicationUserCommandHandler = userCommandHandler;
        _responseBuilder = responseBuilder;
        
    }

    
    [HttpPost]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            
            return BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, errors));
        }
        
        var identityResult = await _applicationUserCommandHandler.ExecuteCreateAsync(command);

        return identityResult.Succeeded switch
        {
            true => Ok(_responseBuilder.CreateResponse((int)HttpStatusCode.Created, identityResult)),
            _ => BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, identityResult.Errors))
        };
    }

}
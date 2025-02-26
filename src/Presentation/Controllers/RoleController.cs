using System.Net;
using Application.Builders;
using Application.CommandHandlers;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands.ApplicationRole;
using Shared.Commands.ApplicationUser;

namespace Presentation.Controllers;

public class RoleController : AuthControllerBase
{
    private readonly IApplicationRoleCommandHandler _applicationRoleCommandHandler;
    private readonly IHttpResponseBuilder _responseBuilder;
    
    public RoleController(IApplicationRoleCommandHandler applicationRoleCommandHandler, IHttpResponseBuilder responseBuilder)
    {
        _applicationRoleCommandHandler = applicationRoleCommandHandler;
        _responseBuilder = responseBuilder;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateRoleCommand command)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, errors));
        }
        
        var identityResult = await _applicationRoleCommandHandler.ExecuteCreateAsync(command);

        return identityResult.Succeeded switch
        {
            true => Ok(_responseBuilder.CreateResponse((int)HttpStatusCode.Created, identityResult)),
            _ => BadRequest(_responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, identityResult.Errors))
        };
    }
    
}
using System.Net;
using Application.Builders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace Presentation.Controllers;

public class RoleController
    (
        ISender sender,
        IHttpResponseBuilder responseBuilder
    ) : AuthControllerBase(responseBuilder)
{
 
    [HttpPost]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateRoleCommand command)
    {
        var result = await sender.Send(command);

        return result switch
        {
            {Succeeded: true} => Ok(responseBuilder.CreateResponse((int)HttpStatusCode.Created, result)),
            _ => BadRequest(responseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result.Errors))
        };
    }
    
}
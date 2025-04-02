using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace Presentation.Controllers;

public class RoleController(
    ISender sender,
    IServiceProvider serviceProvider
) : ControllerBase(serviceProvider)
{
    [HttpPost]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateRoleCommand command)
    {
        var result = await sender.Send(command);

        return result switch
        {
            { Succeeded: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.Created, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result.Errors))
        };
    }
}
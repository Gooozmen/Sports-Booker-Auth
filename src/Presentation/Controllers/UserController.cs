using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.Queries;

namespace Presentation.Controllers;

public class UserController(
    ISender sender,
    IServiceProvider serviceProvider
) : AuthorizeControllerBase(serviceProvider)
{
    [HttpPost]
    public async Task<IActionResult> ProcessUserRegistrationAsync([FromBody] CreateUserCommand command)
    {
        var result = await sender.Send(command);
        return result switch
        {
            { Succeeded: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.Created, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result))
        };
    }

    /// <summary>
    ///  Dynamic endpoint to get a single user by specifying a key and the property type
    /// </summary>
    /// <param name="key">id value, email value, username value</param>
    /// <param name="propertyType">integer id corresponding the key type that is being sent, 3 = id, 4 = name, 6 = email</param>
    /// <returns>UserResponse</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserAsync([FromQuery] string key, [FromQuery] int propertyType)
    {
        var query = new UserQuery(key, propertyType);
        var result = await sender.Send(query);
        return result switch
        {
            { IsSuccess: true } => Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result)),
            { IsSuccess: false } => NotFound(ResponseBuilder.CreateResponse((int)HttpStatusCode.NotFound, result)),
            _ => BadRequest(ResponseBuilder.CreateResponse((int)HttpStatusCode.BadRequest, result))
        };
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserCommand command)
    {
        var userId = UserIdentifyService.GetUserId();
        
        if(userId is null) return BadRequest();
        
        command.Id = Guid.Parse(userId);
        var result = await sender.Send(command);
        return Ok(ResponseBuilder.CreateResponse((int)HttpStatusCode.OK, result));
    }

}
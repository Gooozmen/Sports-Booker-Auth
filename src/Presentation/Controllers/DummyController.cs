using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Auth.Presentation.Controllers;

public class DummyController(
    ISender sender,
    IServiceProvider serviceProvider
) : AuthorizeControllerBase(serviceProvider)
{
    [HttpGet("dummy")]
    public async Task<IActionResult> GetUserAsync()
    {
        return Ok();
    }
}   

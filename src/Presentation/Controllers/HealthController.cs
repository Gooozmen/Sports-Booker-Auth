using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Auth.Presentation.Controllers;

[AllowAnonymous]
[Controller]
[Route("api/[controller]")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("OK");
    }
}
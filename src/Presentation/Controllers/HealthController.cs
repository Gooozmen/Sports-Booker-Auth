using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[AllowAnonymous]
[Controller]
[Route("api/[controller]")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult Get() => Ok("OK");
}


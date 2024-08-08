using Microsoft.AspNetCore.Mvc;

namespace RateLimitingApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RateLimitTestController : ControllerBase
{
    [HttpGet]
    public IActionResult TestEndpoint()
    {
        return Ok();
    }
}

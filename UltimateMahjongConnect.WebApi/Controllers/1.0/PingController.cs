using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ultimate_Mahjong_Connect.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        var result = await Task.FromResult($"Pong from {Assembly.GetExecutingAssembly().GetName().Name} : server is alive !");
        return Ok(result);
    }
}

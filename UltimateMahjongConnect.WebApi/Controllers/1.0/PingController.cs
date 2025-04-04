using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ultimate_Mahjong_Connect.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/ping")]
[ApiController]
public class PingController : ControllerBase
{
    /// <summary>
    /// Check if server is alive
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// Get /api/v1/Ping
    /// </remarks>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        try
        {
            var result = await Task.FromResult($"Pong from {Assembly.GetExecutingAssembly().GetName().Name} : server is alive !");
            return Ok(result);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
}

﻿namespace Ultimate_Mahjong_Connect.Controllers;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        var result = await Task.FromResult($"Pong from {Assembly.GetExecutingAssembly().GetName().Name} : server is alive !");
        return Ok(result);
    }
}

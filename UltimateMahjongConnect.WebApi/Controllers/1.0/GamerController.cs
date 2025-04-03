using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Application.Services;

namespace Ultimate_Mahjong_Connect.Controllers._1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:ApiVersion}/gamers")]
    public class GamerController : Controller
    {
        private readonly GamerService _gamerService;
        public GamerController(GamerService gamerService)
        {
            _gamerService = gamerService;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAllGamers()
        {
            var gamers = await _gamerService.GetAllGamerAsync();
            return Ok(gamers);
        }

        [AllowAnonymous]
        [HttpGet("{pseudonyme}")]
        public async Task<IActionResult> GetGamerByPseudonyme(string pseudonyme)
        {
            var gamer = await _gamerService.GetGamerByPseudonymeAsync(pseudonyme);
            if (gamer == null)
            {
                return NotFound();
            }
            return Ok(gamer);
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> CreateGamer([FromBody] GamerDTO gamerDTO)
        {
            var createdGamerId = await _gamerService.CreateGamerAsync(gamerDTO);
            string? url = Url.Action(
                nameof(CreateGamer),
                "Gamer",
                new { id = createdGamerId },
                Request.Scheme,
                Request.Host.Value
               );
            return Created(url, gamerDTO);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGamerById(int id)
        {
            var gamer = await _gamerService.GetGamerByIdAsync(id);
            if (gamer == null)
            {
                return NotFound();
            }
            return Ok(gamer);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateGamer([FromBody] GamerDTO gamerDTO)
        {
            try
            {
                if (gamerDTO == null)
                {
                    return BadRequest();
                }

                var gamer = await _gamerService.GetGamerByIdAsync(gamerDTO.Id);
                if (gamer == null)
                {
                    return NotFound();
                }
                await _gamerService.UpdateGamerAsync(gamer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


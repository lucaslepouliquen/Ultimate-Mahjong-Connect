using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
        private readonly IGamerService _gamerService;
        public GamerController(IGamerService gamerService)
        {
            _gamerService = gamerService;
        }

        /// <summary>
        /// Get all gamers
        /// </summary>
        /// <returns>A list of all gamers</returns>
        /// <remarks>
        /// Sample request:
        ///     Get /api/v1/gamers
        /// </remarks>
        /// <response code="200">Returns the list of all gamers</response>
        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetAllGamers()
        {
            var gamers = await _gamerService.GetAllGamerAsync();
            return Ok(gamers);
        }

        /// <summary>
        /// Get gamer by pseudonyme
        /// </summary>
        /// <param name="pseudonyme"></param>
        /// <returns></returns>
        /// <response code="200">Returns the gamer by is pseudonyme</response>
        /// <response code="400">The gamer is not found</response>
        [AllowAnonymous]
        [HttpGet("byPseudonyme")]
        public async Task<IActionResult> GetGamerByPseudonyme([FromQuery][Required] string pseudonyme)
        {
            var gamer = await _gamerService.GetGamerByPseudonymeAsync(pseudonyme);
            if (gamer == null)
            {
                return NotFound();
            }
            return Ok(gamer);
        }

        /// <summary>
        /// Create a new gamer
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the new gamer</response>
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

        /// <summary>
        /// Get gamer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the new gamer</response>
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

        /// <summary>
        /// Update the selected gamer
        /// </summary>
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

        /// <summary>
        /// Delete the selected gamer
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGamer(int id)
        {
            try
            {
                var gamer = await _gamerService.GetGamerByIdAsync(id);
                if (gamer == null)
                {
                    return BadRequest();
                }

                await _gamerService.DeleteGamerAsync(gamer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


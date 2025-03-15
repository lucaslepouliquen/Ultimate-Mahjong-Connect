using Microsoft.AspNetCore.Mvc;
using UltimateMahjongConnect.Application.Services;

namespace Ultimate_Mahjong_Connect.Controllers._1._0
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamerController : Controller
    {
        private readonly GamerService _gamerService;
        public GamerController(GamerService gamerService)
        {
            _gamerService = gamerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGamers()
        {
            var gamers = await _gamerService.GetAllGamerAsync();
            return Ok(gamers);
        }

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
    }
}

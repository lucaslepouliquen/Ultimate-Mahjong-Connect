using Microsoft.AspNetCore.Mvc;
using UltimateMahjongConnect.Service.DTO;
using UltimateMahjongConnect.Service.Services;

namespace Ultimate_Mahjong_Connect.Controllers._1._0
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamerController : Controller
    {
        private readonly GamerService _gamerService;
        public  GamerController(GamerService gamerService)
        {
            _gamerService = gamerService;
        }

        [HttpPost("gamers")]
        public async Task<IActionResult> GetAllGamers() 
        {
            var gamers = await _gamerService.GetAllGamerAsync();
            return Ok(gamers);
        }

        [HttpPost]
        public ActionResult CreateGamer([FromBody] GamerDTO gamer)
        {
            return Ok(_gamerService.AddGamerAsync(gamer));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGamerById(int Id)
        {
            var gamer = await _gamerService.GetGamerByIdAsync(Id);
            if(gamer ==null)
            {
                return NotFound();
            }
            return Ok(gamer);
        }
    }
}

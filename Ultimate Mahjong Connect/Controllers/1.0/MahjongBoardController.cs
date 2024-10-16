using Microsoft.AspNetCore.Mvc;
using UltimateMahjongConnect.Core.Net.Interfaces;

namespace Ultimate_Mahjong_Connect.Controllers._1._0
{
    [ApiController]
    [Route("api/[controller]")]
    public class MahjongBoardController : Controller
    {
        private readonly IMahjongBoard _mahjongBoard;
        public MahjongBoardController(IMahjongBoard mahjongBoard)
        {
            _mahjongBoard = mahjongBoard;
        }
        [HttpPost("Initialize/Deterministic")]
        public IActionResult InitializeBoardDeterministically()
        {
            _mahjongBoard.InitializeBoardDeterministically();
            return Ok();
        }

        [HttpPost("Initialize/Random")]
        public IActionResult InitializeBoardRandom()
        {
            _mahjongBoard.InitializeBoardRandom();
            return Ok();
        }

        [HttpPost("Initialize/Random")]
        public IActionResult ReverseInitializeBoardDeterministically()
        {
            _mahjongBoard.ReverseInitializeBoardDeterministically();
            return Ok();
        }
    }
}

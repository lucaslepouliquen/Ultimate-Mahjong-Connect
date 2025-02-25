using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Service.Services;

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
            var board = _mahjongBoard.GetBoard();
            return Ok(board);
        }

        [HttpPost("Initialize/Random")]
        public IActionResult InitializeBoardRandom()
        {
            _mahjongBoard.InitializeBoardRandom();
            var board = _mahjongBoard.GetBoard();
            return Ok(board);
        }

        [HttpPost("Reverse/Initialize/Random")]
        public IActionResult ReverseInitializeBoardDeterministically()
        {
            _mahjongBoard.ReverseInitializeBoardDeterministically();
            var board = _mahjongBoard.GetBoard();
            return Ok(board);
        }
    }
}

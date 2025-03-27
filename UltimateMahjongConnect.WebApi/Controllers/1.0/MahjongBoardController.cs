using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateMahjongConnect.Domain.Interfaces;

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

        [AllowAnonymous]
        [HttpGet("board")]
        public IActionResult InitializeBoardRandom([FromQuery] string mode = "deterministic")
        {
            try
            {
                if (mode == "deterministic")
                {
                    _mahjongBoard.InitializeBoardDeterministically();
                }
                else
                {
                    _mahjongBoard.ReverseInitializeBoardDeterministically();
                }
                return Ok(_mahjongBoard.GetBoard());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [AllowAnonymous]
        [HttpGet("boards/path")]
        public IActionResult GetPath([FromQuery] int row1, [FromQuery] int column1, [FromQuery] int row2, [FromQuery] int column2)
        {
            try {
                var mahjongPath = _mahjongBoard.GetValidatedPath(row1, column1, row2, column2);
                return Ok(mahjongPath);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}

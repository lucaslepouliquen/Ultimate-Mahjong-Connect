using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltimateMahjongConnect.WebApi.Services;

namespace Ultimate_Mahjong_Connect.Controllers._1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:ApiVersion}/board")]
    public class MahjongBoardController : Controller
    {
        private readonly IBoardSessionService _boardSessionService;
        private readonly ILogger<MahjongBoardController> _logger;
        
        public MahjongBoardController(IBoardSessionService boardSessionService, ILogger<MahjongBoardController> logger)
        {
            _boardSessionService = boardSessionService;
            _logger = logger;
        }

        /// <summary>
        /// Get or initialize Mahjong board from session
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Get /api/v1/board/?mode=deterministic
        /// Get /api/v1/board/?mode=random
        /// </remarks>
        [AllowAnonymous]
        [HttpGet()]
        public IActionResult GetBoard([FromQuery] string mode = "deterministic")
        {
            try
            {
                _logger.LogInformation("GetBoard called with mode: {Mode}", mode);
                var board = _boardSessionService.GetOrCreateBoard();
                var boardData = board.GetBoard();
                
                return Ok(boardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBoard: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reset board and create a new one
        /// </summary>
        [AllowAnonymous]
        [HttpPost("reset")]
        public IActionResult ResetBoard([FromQuery] string mode = "deterministic")
        {
            try
            {
                _logger.LogInformation("ResetBoard called with mode: {Mode}", mode);
                _boardSessionService.ClearBoard();
                
                var board = _boardSessionService.GetOrCreateBoard();
                
                return Ok(new { 
                    message = "Board reset successfully",
                    board = board.GetBoard()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ResetBoard: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Check if selected path is valid and execute the move
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Get /api/v1/board/path?row1=1&column1=2&row2=3&column2=4
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("path")]
        public IActionResult ValidateAndExecuteMove([FromQuery] int row1, [FromQuery] int column1, [FromQuery] int row2, [FromQuery] int column2)
        {
            try 
            {
                _logger.LogInformation("ValidateAndExecuteMove called: ({Row1},{Col1}) -> ({Row2},{Col2})", row1, column1, row2, column2);
                
                var board = _boardSessionService.GetOrCreateBoard();
                var mahjongPath = board.GetValidatedPath(row1, column1, row2, column2);
                
                if (mahjongPath.IsValid)
                {
                    board.MatchAndRemoveTiles(board[row1, column1], board[row2, column2]);
                    
                    _boardSessionService.SaveBoard(board);
                    _logger.LogInformation("Move executed and board saved");
                    
                    return Ok(new { 
                        isValid = true, 
                        path = mahjongPath,
                        board = board.GetBoard(),
                        message = "Move executed successfully"
                    });
                }
                else
                {
                    _logger.LogInformation("Invalid move attempted");
                    return Ok(new { 
                        isValid = false, 
                        path = mahjongPath,
                        message = "Invalid move"
                    });
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ValidateAndExecuteMove: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Debug endpoint to check session state (Development only)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("debug")]
        public IActionResult GetDebugInfo()
        {
            // Désactiver en production
            if (!HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                return NotFound();
            }

            try
            {
                var hasBoard = _boardSessionService.HasBoard();
                var sessionId = HttpContext.Session.Id;
                var sessionKeys = HttpContext.Session.Keys.ToList();
                var cookies = HttpContext.Request.Cookies
                    .Where(c => c.Key.Contains("Session") || c.Key.Contains("Mahjong"))
                    .ToDictionary(c => c.Key, c => c.Value);
                
                return Ok(new
                {
                    SessionId = sessionId,
                    HasBoardInSession = hasBoard,
                    SessionKeys = sessionKeys,
                    IncomingCookies = cookies,
                    Timestamp = DateTime.UtcNow,
                    Environment = "Development"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDebugInfo: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
        }
}

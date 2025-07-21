using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.WebApi.Services
{
    public interface IBoardSessionService
    {
        IMahjongBoard GetOrCreateBoard();
        void SaveBoard(IMahjongBoard board);
        void ClearBoard();
        bool HasBoard();
    }

    public class BoardSessionService : IBoardSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMahjongTile _mahjongTile;
        private readonly ILogger<BoardSessionService> _logger;
        private const string BOARD_SESSION_KEY = "MahjongBoard";

        public BoardSessionService(IHttpContextAccessor httpContextAccessor, IMahjongTile mahjongTile, ILogger<BoardSessionService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _mahjongTile = mahjongTile;
            _logger = logger;
        }

        public IMahjongBoard GetOrCreateBoard()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                throw new InvalidOperationException("Session not available");

            var boardJson = session.GetString(BOARD_SESSION_KEY);
            
            if (!string.IsNullOrEmpty(boardJson))
            {
                try
                {
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        DefaultValueHandling = DefaultValueHandling.Include,
                        Formatting = Formatting.None
                    };
                    
                    var boardData = JsonConvert.DeserializeObject<MahjongBoardData>(boardJson, jsonSettings);
                    
                    if (boardData?.Board != null)
                    {
                        var board = new MahjongBoard(_mahjongTile);
                        board.LoadFromData(boardData);
                        return board;
                    }
                    else
                    {
                        session.Remove(BOARD_SESSION_KEY);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deserializing board from session");
                    session.Remove(BOARD_SESSION_KEY);
                }
            }

            var newBoard = new MahjongBoard(_mahjongTile);
            newBoard.InitializeBoardDeterministically();
            SaveBoard(newBoard);
            return newBoard;
        }

        public void SaveBoard(IMahjongBoard board)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                throw new InvalidOperationException("Session not available");

            try
            {
                var boardData = ((MahjongBoard)board).ToData();
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    Formatting = Formatting.None
                };
                
                var boardJson = JsonConvert.SerializeObject(boardData, jsonSettings);
                session.SetString(BOARD_SESSION_KEY, boardJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving board to session");
                throw;
            }
        }

        public void ClearBoard()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(BOARD_SESSION_KEY);
                _logger.LogInformation("ClearBoard: Board cleared from session");
            }
        }

        public bool HasBoard()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session != null && !string.IsNullOrEmpty(session.GetString(BOARD_SESSION_KEY));
        }
    }
}
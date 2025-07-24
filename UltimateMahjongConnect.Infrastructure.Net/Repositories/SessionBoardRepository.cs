using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Infrastructure.Repositories
{
    public class SessionBoardRepository : IBoardRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMahjongTile _mahjongTile;
        private readonly ILogger<SessionBoardRepository> _logger;
        private const string BOARD_SESSION_KEY = "MahjongBoard";

        public SessionBoardRepository(
            IHttpContextAccessor httpContextAccessor,
            IMahjongTile mahjongTile,
            ILogger<SessionBoardRepository> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _mahjongTile = mahjongTile;
            _logger = logger;
        }

        public IMahjongBoard? GetBoard()
        {
            var session = GetSession();
            if (session == null) return null;

            var boardJson = session.GetString(BOARD_SESSION_KEY);
            if (string.IsNullOrEmpty(boardJson))
                return null;

            try
            {
                return DeserializeBoard(boardJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing board from session");
                session.Remove(BOARD_SESSION_KEY);
                return null;
            }
        }

        public void SaveBoard(IMahjongBoard board)
        {
            var session = GetSession();
            if (session == null)
                throw new InvalidOperationException("Session not available");

            try
            {
                var boardJson = SerializeBoard(board);
                session.SetString(BOARD_SESSION_KEY, boardJson);

                _logger.LogDebug("Board serialized and saved to session");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving board to session");
                throw;
            }
        }

        public void RemoveBoard()
        {
            var session = GetSession();
            if (session == null) return;

            session.Remove(BOARD_SESSION_KEY);
            _logger.LogDebug("Board removed from session");
        }

        public bool HasBoard()
        {
            var session = GetSession();
            if (session == null) return false;

            return !string.IsNullOrEmpty(session.GetString(BOARD_SESSION_KEY));
        }

        private ISession? GetSession()
        {
            return _httpContextAccessor.HttpContext?.Session;
        }

        private IMahjongBoard DeserializeBoard(string boardJson)
        {
            var jsonSettings = GetJsonSettings();
            var boardData = JsonConvert.DeserializeObject<MahjongBoardData>(boardJson, jsonSettings);

            if (boardData?.Board == null)
                throw new InvalidOperationException("Invalid board data in session");

            var board = new MahjongBoard(_mahjongTile);
            board.LoadFromData(boardData);
            return board;
        }

        private string SerializeBoard(IMahjongBoard board)
        {
            var boardData = ((MahjongBoard)board).ToData();
            var jsonSettings = GetJsonSettings();
            return JsonConvert.SerializeObject(boardData, jsonSettings);
        }

        private JsonSerializerSettings GetJsonSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.None
            };
        }
    }
}
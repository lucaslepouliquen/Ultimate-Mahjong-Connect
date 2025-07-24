using Microsoft.Extensions.Logging;
using UltimateMahjongConnect.Application.Services;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMahjongTile _mahjongTile;
        private readonly ILogger<BoardService> _logger;

        public BoardService(
            IBoardRepository boardRepository,
            IMahjongTile mahjongTile,
            ILogger<BoardService> logger)
        {
            _boardRepository = boardRepository;
            _mahjongTile = mahjongTile;
            _logger = logger;
        }

        public IMahjongBoard GetOrCreateBoard()
        {
            var board = _boardRepository.GetBoard();

            if (board != null)
            {
                _logger.LogDebug("Board loaded from repository");
                return board;
            }

            // Créer un nouveau board
            var newBoard = CreateNewBoard();
            _boardRepository.SaveBoard(newBoard);

            _logger.LogInformation("New board created and saved");
            return newBoard;
        }

        public void SaveBoard(IMahjongBoard board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            _boardRepository.SaveBoard(board);
            _logger.LogDebug("Board saved to repository");
        }

        public void ClearBoard()
        {
            _boardRepository.RemoveBoard();
            _logger.LogInformation("Board cleared from repository");
        }

        public bool HasBoard()
        {
            return _boardRepository.HasBoard();
        }

        private IMahjongBoard CreateNewBoard()
        {
            var board = new MahjongBoard(_mahjongTile);
            board.InitializeBoardDeterministically();
            return board;
        }
    }
}

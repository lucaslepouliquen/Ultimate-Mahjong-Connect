using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Services
{
    public interface IBoardService
    {
        IMahjongBoard GetOrCreateBoard();
        void SaveBoard(IMahjongBoard board);
        void ClearBoard();
        bool HasBoard();
    }
}
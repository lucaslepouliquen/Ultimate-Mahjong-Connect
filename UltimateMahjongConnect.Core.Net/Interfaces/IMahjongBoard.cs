using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongBoard
    {
        void InitializeBoardDeterministically();
        void TransposeBoard();
        void InitializeBoardRandom();
        IMahjongTile this[int row, int col] { get; }
    }
}

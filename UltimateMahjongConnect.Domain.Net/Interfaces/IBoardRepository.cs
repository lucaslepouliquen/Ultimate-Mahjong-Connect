namespace UltimateMahjongConnect.Domain.Interfaces
{
    public interface IBoardRepository
    {
        IMahjongBoard? GetBoard();
        void SaveBoard(IMahjongBoard board);
        void RemoveBoard();
        bool HasBoard();
    }
}
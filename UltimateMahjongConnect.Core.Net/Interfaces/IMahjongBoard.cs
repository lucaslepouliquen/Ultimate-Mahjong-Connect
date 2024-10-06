namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongBoard
    {
        void InitializeBoardDeterministically();
        void TransposeBoard();
        void InitializeBoardRandom();
    }
}

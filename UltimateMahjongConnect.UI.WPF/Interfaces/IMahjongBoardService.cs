namespace UltimateMahjongConnect.UI.WPF.Interfaces
{
    public interface IMahjongBoardService
    {
        void InitializeBoardDeterministically();
        void InitializeBoardRandomly();
    }
}
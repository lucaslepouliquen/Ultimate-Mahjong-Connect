using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.UI.WPF.Interfaces;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF.Services
{
    public class MahjongBoardService : IMahjongBoardService
    {
        private readonly IMahjongBoard _mahjongBoard;
        public MahjongBoardService(IMahjongBoard mahjongBoard)
        {
            _mahjongBoard = mahjongBoard  ?? throw new ArgumentNullException(nameof(mahjongBoard));
        }
        public void InitializeBoardDeterministically()
        {
            _mahjongBoard.InitializeBoardDeterministically();
        }

        public void InitializeBoardRandomly()
        {
            _mahjongBoard.InitializeBoardRandom();
        }

        
    }
}

using System.Collections.ObjectModel;
using System.Windows.Input;
using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MahjongBoardViewModel : ViewModelBase
    {
        private readonly IMahjongBoard _mahjongBoard;
        public ObservableCollection<MahjongTileViewModel> Tiles { get; set; }

        public ICommand InitializeRandomCommand { get; }

        public MahjongBoardViewModel(IMahjongBoard mahjongBoard)
        {
            _mahjongBoard = mahjongBoard;
            Tiles = new ObservableCollection<MahjongTileViewModel>();

            InitializeRandomCommand = new RelayCommand(_ => InitializeRandomBoard());
        }

        private void InitializeRandomBoard()
        {
            _mahjongBoard.InitializeBoardRandom();
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            Tiles.Clear();

            for (int row = 0; row < 14; row++)
            {
                for (int col = 0; col < 14; col++)
                {
                    var tile = _mahjongBoard[row, col];
                    Tiles.Add(new MahjongTileViewModel(tile, row, col));
                }
            }
        }

        public async override Task LoadAsync()
        {
            await Task.CompletedTask;
        }
    }
}

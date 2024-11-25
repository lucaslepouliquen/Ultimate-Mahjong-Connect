using System.Collections.ObjectModel;
using System.Windows.Input;
using UltimateMahjongConnect.Business.Models;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MahjongBoardViewModel : ViewModelBase
    {
        private readonly Gamer _gamer;
        private readonly IMahjongBoard _mahjongBoard;
        private MahjongTileViewModel? _selectedTile1;
        private MahjongTileViewModel? _selectedTile2;
        public ObservableCollection<MahjongTileViewModel> Tiles { get; set; }
        public ICommand InitializeRandomCommand { get; }
        public ICommand TileCommand { get; }
        public int Score => _gamer.Score;

        public MahjongBoardViewModel(IMahjongBoard mahjongBoard, Gamer gamer)
        {
            _gamer = gamer;
            _mahjongBoard = mahjongBoard;
            Tiles = new ObservableCollection<MahjongTileViewModel>();
            TileCommand = new AsyncRelayCommand<MahjongTileViewModel>(OnTileClicked);
            InitializeRandomCommand = new RelayCommand(_ => InitializeRandomBoard());
        }

        private void InitializeBoardDeterministically()
        {
            _mahjongBoard.InitializeBoardDeterministically();
            UpdateTiles();
        }

        public void InitializeRandomBoard()
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
            InitializeRandomBoard();
            await Task.CompletedTask;
        }

        private async Task OnTileClicked(MahjongTileViewModel clickedTile)
        {
            if (_selectedTile1 == null)
            {
                _selectedTile1 = clickedTile;
            }
            else if (_selectedTile2 == null)
            {
                _selectedTile2 = clickedTile;
                var mahjongPath = _mahjongBoard.GetValidatedPath(_selectedTile1.Row, _selectedTile1.Column, _selectedTile2.Row, _selectedTile2.Column);
                bool isPathValid = mahjongPath.IsValid;
                if (isPathValid) {
                    var pathTiles = GetPathTiles(mahjongPath);
                    await HighlightAndRemoveTilesIfPathValid(pathTiles);
                    _gamer.AddScore(10);
                    _mahjongBoard.MatchTiles(_selectedTile1.GetTile(), _selectedTile2.GetTile());
                    RaisePropertyChanged(nameof(Score));
                    UpdateView();
                }
                _selectedTile1 = null;
                _selectedTile2 = null;
            }
        }

        private void UpdateView()
        {
            for (int row = 0; row < _mahjongBoard.GetRows(); row++)
            {
                for (int col = 0; col < _mahjongBoard.GetColumns(); col++)
                {
                    var tile = _mahjongBoard[row, col];
                    UpdateTileUI(row, col, tile);
                }
            }
        }

        private void UpdateTileUI(int row, int col, IMahjongTile tile)
        {
            int index = row * _mahjongBoard.GetRows() + col;

            if (index >= 0 && index < Tiles.Count)

                Tiles[index] = new MahjongTileViewModel(tile, row, col);
        }

        public async Task HighlightAndRemoveTilesIfPathValid(List<MahjongTileViewModel> pathTiles)
        {
            foreach (var tile in pathTiles)
            {
                tile.IsPathHighlighted = true;
            }

            await Task.Delay(500); 

            foreach (var tile in pathTiles)
            {
                tile.IsPathHighlighted = false; 
            }
        }

        private List<MahjongTileViewModel> GetPathTiles(MahjongPath mahjongPath)
        {
            var pathTiles = new List<MahjongTileViewModel>();

            for (int i = 0; i < mahjongPath.PathRows.Count; i++)
            {
                int row = mahjongPath.PathRows[i];
                int col = mahjongPath.PathColumns[i];

                int index = row * _mahjongBoard.GetColumns() + col;

                if (index >= 0 && index < Tiles.Count)
                {
                    pathTiles.Add(Tiles[index]);
                }
            }
            return pathTiles;
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
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
            TileCommand = new RelayCommand<MahjongTileViewModel>(OnTileClicked);
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
            InitializeRandomBoard();
            await Task.CompletedTask;
        }

        private void OnTileClicked(MahjongTileViewModel clickedTile)
        {
            if (_selectedTile1 == null)
            {
                _selectedTile1 = clickedTile;
            }
            else if (_selectedTile2 == null)
            {
                _selectedTile2 = clickedTile;
                if (_mahjongBoard.IsPathValid(_selectedTile1.Row, _selectedTile1.Column, _selectedTile2.Row, _selectedTile2.Column))
                {
                    _gamer.AddScore(10);
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
                tile.IsPathLighted = true;
            }

            await Task.Delay(500); 

            foreach (var tile in pathTiles)
            {
                tile.IsPathLighted = false; 
            }
        }
    }
}
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using UltimateMahjongConnect.Business.Models;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.UI.WPF.Interfaces;
using UltimateMahjongConnect.UI.WPF.Services;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MahjongBoardViewModel : ViewModelBase
    {
        private readonly Gamer _gamer;
        private readonly IMahjongBoard _mahjongBoard;
        private readonly IMahjongBoardService _mahjongBoardService;
        private MahjongTileViewModel? _selectedTile1;
        private MahjongTileViewModel? _selectedTile2;
        public ObservableCollection<MahjongTileViewModel> Tiles { get; set; }
        public ICommand InitializeDeterministicallyCommand { get; }
        public ICommand InitializeRandomCommand { get; }
        public ICommand TileCommand { get; }
        public int Score => _gamer.Score;

        public MahjongBoardViewModel(IMahjongBoard mahjongBoard, Gamer gamer, IMahjongBoardService mahjongBoardService)
        {
            _gamer = gamer;
            _mahjongBoard = mahjongBoard ?? throw new ArgumentNullException(nameof(mahjongBoard));
            _mahjongBoard.InitializeBoardDeterministically();
            _mahjongBoardService = mahjongBoardService;
            Tiles = new ObservableCollection<MahjongTileViewModel>();
            TileCommand = new AsyncRelayCommand<MahjongTileViewModel>(OnTileClicked);
            InitializeRandomCommand = new RelayCommand(_ => InitializeRandomBoard());
            InitializeDeterministicallyCommand = new RelayCommand(_ => InitializeBoardDeterministically());
        }

        private void InitializeBoardDeterministically()
        {
            _mahjongBoardService.InitializeBoardDeterministically();
            LoadTilesFromBoard();
        }

        public void InitializeRandomBoard()
        {
            _mahjongBoardService.InitializeBoardRandomly();
            LoadTilesFromBoard();
        }

        public async override Task LoadAsync()
        {
            InitializeBoardDeterministically();
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
                    _mahjongBoard.MatchAndRemoveTiles(_selectedTile1.GetTile(), _selectedTile2.GetTile());
                    RaisePropertyChanged(nameof(Score));
                }
                _selectedTile1 = null;
                _selectedTile2 = null;
            }
        }

        private void LoadTilesFromBoard()
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
using System.Windows.Input;
using UltimateMahjongConnect.Business.Models;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Domain.Interfaces;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MahjongTileViewModel : ViewModelBase
    {
        private readonly IMahjongTile _tile;
        public int Row { get; }
        public int Column { get; }

        public string DisplayText => _tile.ToString();
        private bool _isPathHighlighted;
        public bool IsPathHighlighted
        {
            get => _isPathHighlighted;
            set
            {
                _isPathHighlighted = value;
                RaisePropertyChanged(nameof(IsPathHighlighted));
            }
        }

        public ICommand TileCommand { get; }
        public MahjongTileViewModel(IMahjongTile tile, int row, int column)
        {
            _tile = tile;
            Row = row;
            Column = column;
            _tile.TileChanged += OnTileChanged;
        }

        private void OnTileChanged()
        {
            RaisePropertyChanged(nameof(DisplayText));
        }

        ~MahjongTileViewModel()
        {
            _tile.TileChanged -= OnTileChanged;
        }

        public IMahjongTile GetTile()
        {
            return _tile;
        }
    }
}
using System.Windows.Input;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MahjongTileViewModel : ViewModelBase
    {
        private readonly IMahjongTile _tile;
        public int Row { get; }
        public int Column { get; }

        public string DisplayText => _tile.ToString();

        public bool IsPathLighted { get; set; } = false;

        public ICommand TileCommand { get; }
        public MahjongTileViewModel(IMahjongTile tile, int row, int column)
        {
            _tile = tile;
            Row = row;
            Column = column;
        }
    }
}
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongTile
    {
        event Action TileChanged;
        List<MahjongTile> GetTiles();
        bool CanBeMatched(MahjongTile otherTile);
        bool IsRemoved{ get; set; }
        void ResetState();
        void MarkAsMatchedAndRemoved();
        string DisplayText { get; }
    }
}

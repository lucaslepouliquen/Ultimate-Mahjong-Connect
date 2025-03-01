using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Domain.Interfaces
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

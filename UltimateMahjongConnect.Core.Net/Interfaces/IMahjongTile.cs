using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongTile
    {
        List<MahjongTile> GetTiles();
        bool CanBeMatched(MahjongTile otherTile);
        void ResetState();
        void MarkAsMatched();
    }
}

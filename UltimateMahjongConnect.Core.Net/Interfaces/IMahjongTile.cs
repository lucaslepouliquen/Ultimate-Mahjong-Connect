using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Core.Net.Interfaces
{
    public interface IMahjongTile
    {
        List<MahjongTile> GetTiles();
        bool CanBeMatched(MahjongTile otherTile);
        bool IsRemoved{ get;}
        void ResetState();
        void MarkAsMatched();
    }
}

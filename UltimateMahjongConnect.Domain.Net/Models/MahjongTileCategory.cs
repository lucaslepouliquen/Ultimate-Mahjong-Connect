namespace UltimateMahjongConnect.Core.Net.Models
{
    public enum MahjongTileCategory
    {
        [TileRepetition(4)] Bamboo,
        [TileRepetition(4)] Circles,
        [TileRepetition(4)] Characters,
        [TileRepetition(4)] Winds,
        [TileRepetition(4)] Dragons,
        [TileRepetition(1)] Flowers,
        [TileRepetition(1)] Seasons
    }
}

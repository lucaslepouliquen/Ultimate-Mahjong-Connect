using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Test
{
    public class MahjongTileTests
    {
        [Fact]
        public void ShouldReturns144Tiles()
        {
            // Arrange
            var mahjongTile = new MahjongTile();

            // Act
            var tiles = mahjongTile.GetTiles();

            // Assert
            Assert.Equal(144, tiles.Count);
        }
    }
}
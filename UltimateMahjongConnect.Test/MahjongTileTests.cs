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

        [Theory]
        [InlineData(MahjongTileCategory.Bamboo, 1, MahjongTileCategory.Bamboo, 1, true)]
        [InlineData(MahjongTileCategory.Bamboo, 1, MahjongTileCategory.Bamboo, 2, false)]
        [InlineData(MahjongTileCategory.Bamboo, 1, MahjongTileCategory.Circles, 1, false)]
        public void ShouldTilesBeMatched_WhenTilesAreTheSame(MahjongTileCategory category1, int value1, MahjongTileCategory category2, int value2, bool expectedResult)
        {
            // Arrange
            var tile1 = new MahjongTile(category1, value1);
            var tile2 = new MahjongTile(category2, value2);

            // Act
            var result = tile1.CanBeMatched(tile2);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
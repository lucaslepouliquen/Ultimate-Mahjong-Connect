using System.Collections.Generic;
using Xunit;
using Moq;
using UltimateMahjongConnect.Core.Net;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Test
{
    public class MahjongTileProviderTests
    {
        [Fact]
        public void GetTiles_Returns144Tiles()
        {
            // Arrange
            var mockMahjongTileProvider = new Mock<IMahjongTile>();
            mockMahjongTileProvider.Setup(provider => provider.GetTiles()).Returns(CreateTestTiles());

            var mahjongTileProvider = mockMahjongTileProvider.Object;

            // Act
            var tiles = mahjongTileProvider.GetTiles();

            // Assert
            Assert.Equal(144, tiles.Count);
        }

        private List<MahjongTile> CreateTestTiles()
        {
            var tiles = new List<MahjongTile>();

            // Ajoutez ici la logique de création des tuiles de test en fonction des règles du Mahjong

            return tiles;
        }
    }
}
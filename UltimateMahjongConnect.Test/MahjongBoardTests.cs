using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Test.Core.Net
{
    public class MahjongBoardTests
    {
        [Fact]
        public void ShouldInitializeBoardDeterministically()
        {
            // Arrange
            var mahjongTile = new MahjongTile();
            var board = new MahjongBoard(mahjongTile);

            // Act
            board.InitializeBoardDeterministically();

            // Assert
            var expectedTiles = mahjongTile.GetTiles();
            int index = 0;

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    var tileOnBoard = board[i, j];
                    var expectedTile = expectedTiles[index++];

                    Assert.Equal(expectedTile.Category, tileOnBoard.Category);
                    Assert.Equal(expectedTile.Value, tileOnBoard.Value);
                }
            }
        }
    }
}

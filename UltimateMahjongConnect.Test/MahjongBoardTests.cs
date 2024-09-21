using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Test.Core.Net
{
    public class MahjongBoardTests
    {
        private readonly IMahjongTile _mahjongTile;

        public MahjongBoardTests()
        {
            _mahjongTile = new MahjongTile();    
        }

        [Fact]
        public void ShouldInitializeBoardDeterministically()
        {
            // Arrange
            var board = new MahjongBoard(_mahjongTile);

            // Act
            board.InitializeBoardDeterministically();

            // Assert
            var expectedTiles = _mahjongTile.GetTiles();
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

        [Fact]
        public void ShouldValidatePathBetweenTwoAdjacentTiles()
        {
            //Arrange
            var board = new MahjongBoard(_mahjongTile);

            //Act
            board.InitializeBoardDeterministically();

            //Assert
            Assert.True(board.IsPathValid(0, 0, 0, 1));
            Assert.False(board.IsPathValid(0, 0, 0, 10));
        }
    }
}

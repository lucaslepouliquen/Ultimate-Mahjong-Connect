using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Test.Core.Net
{
    public class MahjongBoardTests
    {
        private readonly IMahjongTile _mahjongTile;
        private MahjongBoard _board;

        public MahjongBoardTests()
        {
            _mahjongTile = new MahjongTile();
            _board = new MahjongBoard(_mahjongTile);
        }

        private void InitializeBoardDeterministically()
        {
            _board.InitializeBoardDeterministically();
        }

        [Fact]
        public void ShouldInitializeBoardDeterministically()
        {
            InitializeBoardDeterministically();

            var expectedTiles = _mahjongTile.GetTiles();
            int index = 0;

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    var tileOnBoard = _board[i, j];
                    var expectedTile = expectedTiles[index++];

                    Assert.Equal(expectedTile.Category, tileOnBoard.Category);
                    Assert.Equal(expectedTile.Value, tileOnBoard.Value);
                }
            }
        }

        [Fact]
        public void ShouldValidatePathBetweenAdjacentMatchingTiles()
        {
            InitializeBoardDeterministically();
            bool isValid = _board.IsPathValid(0, 0, 0, 1);
            Assert.True(isValid, "Path between adjacent matching tiles should be valid.");
        }


        [Fact]
        public void ShouldValidatePathBetweenNonAdjacentMatchingTilesIfPathIsClear()
        {
            InitializeBoardDeterministically();
            bool isValid = _board.IsPathValid(0, 0, 0, 2);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldMarkTilesAsRemovedWhenPathIsValid()
        {
            InitializeBoardDeterministically();
            Assert.True(_board.IsPathValid(0, 0, 0, 1));
            Assert.True(_board[0, 0].IsRemoved);
            Assert.True(_board[0, 1].IsRemoved);
        }

        [Fact]
        public void ShouldNotValidatePathBetweenMatchingTileWhenPathIsNotCleared()
        {
            InitializeBoardDeterministically();

            Assert.False(_board.IsPathValid(1, 0, 1, 3), "Path between matching tiles should not be valid if it's blocked by other tiles");
        }
    }
}

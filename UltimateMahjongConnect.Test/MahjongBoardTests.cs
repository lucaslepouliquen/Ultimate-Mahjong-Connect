using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Test
{
    public class MahjongBoardTests
    {
        private readonly IMahjongTile _mahjongTile;
        private MahjongBoard _board;
        private readonly int _rows = 14;
        private readonly int _columns = 14;

        public MahjongBoardTests()
        {
            _mahjongTile = new MahjongTile();
            _board = new MahjongBoard(_mahjongTile);
        }

        private void InitializeBoardDeterministically()
        {
            _board.InitializeBoardDeterministically();
        }

        private void InitializeBoardDeterministicallyVertically()
        {
            _board.ReverseInitializeBoardDeterministically();
        }

        [Fact]
        public void ShouldInitializeBoardWithRemovedBorder()
        {
            InitializeBoardDeterministically();

            // Vérifier les coins de la bordure
            for (int i = 0; i < _columns; i++)
            {
                Assert.True(_board[0, i].IsRemoved, "La tuile de bordure doit être marquée comme 'Removed'.");
                Assert.True(_board[_rows - 1, i].IsRemoved, "La tuile de bordure doit être marquée comme 'Removed'.");
            }

            for (int i = 0; i < _rows; i++)
            {
                Assert.True(_board[i, 0].IsRemoved, "La tuile de bordure doit être marquée comme 'Removed'.");
                Assert.True(_board[i, _columns - 1].IsRemoved, "La tuile de bordure doit être marquée comme 'Removed'.");
            }
        }

        [Fact]
        public void ShouldInitializeBoardDeterministically()
        {
            InitializeBoardDeterministically();

            var expectedTiles = _mahjongTile.GetTiles();
            int index = 0;

            for (int i = 1; i < _rows - 1; i++)
            {
                for (int j = 1; j < _columns - 1; j++)
                {
                    var tileOnBoard = _board[i, j];
                    var expectedTile = expectedTiles[index++];

                    Assert.Equal(expectedTile.Category, tileOnBoard.Category);
                    Assert.Equal(expectedTile.Value, tileOnBoard.Value);
                }
            }
        }

        [Fact]
        public void GetValidatedPath_ShouldReturnInvalidForSingleTile()
        {
            InitializeBoardDeterministically();
            var mahjongPath = _board.GetValidatedPath(1, 1, 1, 1);
            Assert.False(mahjongPath.IsValid);
        }

        [Fact]
        public void ShouldInvalidatePathForSingleTile()
        {
            InitializeBoardDeterministically();
            bool isValid = _board.IsPathValid(1, 1, 1, 1);
            Assert.False(isValid, "A path starting and ending on the same tile should be invalid.");
        }

        [Fact]
        public void ShouldValidatePathHorizontallyBetweenAdjacentMatchingTiles()
        {
            InitializeBoardDeterministically();
            bool isValid = _board.IsPathValid(1, 1, 1, 2);
            Assert.True(isValid, "Path between horizontally adjacent matching tiles should be valid.");
        }

        [Fact]
        public void ShouldValidateVerticallyBetweenAdjacentMatchingTiles()
        {
            InitializeBoardDeterministicallyVertically();
            bool isValid = _board.IsPathValid(2, 1, 1, 1);
            Assert.True(isValid, "Path between horizontally adjacent matching tiles should be valid.");
        }

        [Fact]
        public void ShouldValidatePathBetweenVerticallyAdjacentMatchingTiles()
        {
            InitializeBoardDeterministicallyVertically();
            bool isValid = _board.IsPathValid(1, 1, 2, 1);
            Assert.True(isValid, "Path between vertically adjacent matching tiles should be valid.");
        }

        [Fact]
        public void ShouldValidatePathBetweenNonAdjacentMatchingTilesIfPathIsClear()
        {
            InitializeBoardDeterministically();
            bool isValid = true;
            for (int i = 1; i < 13; i++)
            {
                if (isValid)
                {
                    isValid = _board.IsPathValid(i, 1, i, 3);
                }
            }
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldValidatePathBetweenTilesAtSpecificPositions()
        {
            _board.SetupBoardWithTilesInSpecificPattern();
            var isValid = _board.IsPathValid(1, 1, 1, 4);
            Assert.True(isValid, "Path between vertically adjacent matching tiles should be valid.");
        }

        [Fact]
        public void PathValidation_ForMatchingTilesInRowRange_ShouldReturnTrue()
        {
            _board.SetupBoardWithTilesInRowRange();
            var isValid = _board.IsPathValid(1, 1, 1, 4);
            Assert.True(isValid, "Path between vertically adjacent matching tiles should be valid.");
        }

        [Fact]
        public void ShouldValidatePathBetweenNonAdjacentTilesWithSameCategory()
        {
            InitializeBoardDeterministically();
            var isValid = _board.IsPathValid(1, 1, 1, 4);
            Assert.Equal(_board[1, 1].Category, _board[1, 4].Category);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldValidatePathBetween_SouthWest_NorthEast()
        {
            _board.SetupBoardWithTilesInSpecificPattern_SouthWest_NorthEast();
            var isValid = _board.IsPathValid(11, 3, 5, 12);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldValidatePathBetween_SouthEast_NorthWest()
        {
            _board.SetupBoardWithTilesInSpecificPattern_SouthWest_NorthEast();
            var isValid = _board.IsPathValid(11, 12, 5, 3);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldValidatePathBetween_NorthWest_SouthEast()
        {
            _board.SetupBoardWithTilesInSpecificPattern_SouthWest_NorthEast();
            var isValid = _board.IsPathValid(5, 3, 11, 12);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldValidatePathBetween_NorthEast_SouthWest()
        {
            _board.SetupBoardWithTilesInSpecificPattern_NorthEast_SouthWest();
            var isValid = _board.IsPathValid(5, 12, 11, 5);
            Assert.True(isValid, "Path between non-adjacent matching tiles should be valid if path is clear.");
        }

        [Fact]
        public void ShouldMarkTilesAsRemovedWhenPathIsValid()
        {
            InitializeBoardDeterministically();
            Assert.True(_board.IsPathValid(1, 1, 1, 2));
            Assert.True(_board[0, 0].IsRemoved);
            Assert.True(_board[0, 1].IsRemoved);
        }

        [Fact]
        public void ShouldNotValidatePathBetweenMatchingTileWhenPathIsNotCleared()
        {
            InitializeBoardDeterministically();

            Assert.False(_board.IsPathValid(2, 1, 2, 4), "Path between matching tiles should not be valid if it's blocked by other tiles");
        }
    }
}

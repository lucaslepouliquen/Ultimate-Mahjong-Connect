using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongBoard : IMahjongBoard
    {

        private readonly int _rows = 14;
        private readonly int _columns = 14;
        private readonly IMahjongTile _mahjongTile;
        private IMahjongTile[,] _board;

        IMahjongTile IMahjongBoard.this[int row, int col] => this[row, col];

        public MahjongBoard(IMahjongTile mahjongTile)
        {
            _mahjongTile = mahjongTile;
        }

        public int GetRows() => _rows;

        public int GetColumns() => _columns;
        public void InitializeBoardDeterministically() => InitializeBoard(randomize: false);

        public void InitializeBoardRandom() => InitializeBoard(randomize: true);

        private void InitializeBoard(bool randomize)
        {
            _board = new MahjongTile[_rows, _columns];
            var tiles = _mahjongTile.GetTiles();

            foreach (var tile in tiles)
            {
                tile.ResetState();
            }

            if (tiles.Count < (_rows -2) * (_columns -2))
            {
                throw new InvalidOperationException("Not enough tiles to fill the board");
            }

            var random = new Random();

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 0 || i == _rows - 1 || j == 0 || j == _columns - 1)
                    {
                        _board[i, j] = new MahjongTile(true);
                    }
                    else
                    {
                        int tileIndex = randomize ? random.Next(tiles.Count) : 0;
                        var randomTile = tiles[tileIndex];

                        _board[i, j] = randomTile;
                        tiles.RemoveAt(tileIndex);
                    }
                }
            }
        }

        public void ReverseInitializeBoardDeterministically()
        {
            InitializeBoardDeterministically();
            TransposeBoard();
        }

        public void TransposeBoard()
        {
            var transposedBoard = new IMahjongTile[_columns, _rows];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    transposedBoard[j, i] = _board[i, j];
                }
            }
            _board = transposedBoard;
        }

        public bool IsPathValid(int row1, int column1, int row2, int column2)
        {
            if (_board[row1, column1] is MahjongTile tile1 &&
                _board[row2, column2] is MahjongTile tile2 &&
                tile1.CanBeMatched(tile2))
            {
                if (row1 == row2 && IsHorizontalPathClear(row1, column1, column2))
                {
                    MatchTiles(tile1, tile2);
                    return true;
                }

                if (column1 == column2 && IsVerticalPathClear(column1, row1, row2))
                {
                    MatchTiles(tile1, tile2);
                    return true;
                }

                if (IsLShapedPathClear(row1, column1, row2, column2))
                {
                    MatchTiles(tile1, tile2);
                    return true;
                }

                if (IsThreeSegmentPathClear(row1, column1, row2, column2))
                {
                    MatchTiles(tile1, tile2);
                    return true;
                }

                return false;
            }
            return false;
        }

        private bool IsHorizontalPathClear(int row, int column1, int column2)
        {
            int start = Math.Min(column1, column2) + 1;
            int end = Math.Max(column1, column2);

            for (int col = start; col < end; col++)
            {
                if (_board[row, col] != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsVerticalPathClear(int column, int row1, int row2)
        {
            int start = Math.Min(row1, row2) + 1;
            int end = Math.Max(row1, row2);

            for (int row = start; row < end; row++)
            {
                if (_board[row, column] != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsLShapedPathClear(int row1, int column1, int row2, int column2)
        {
            if (IsHorizontalPathClear(row1, column1, column2) && IsVerticalPathClear(column2, row1, row2))
            {
                return true;
            }

            if (IsVerticalPathClear(column1, row1, row2) && IsHorizontalPathClear(row2, column1, column2))
            {
                return true;
            }

            return false;
        }

        private bool IsThreeSegmentPathClear(int row1, int column1, int row2, int column2)
        {
            for (int intermediateRow = 0; intermediateRow < _rows; intermediateRow++)
            {
                if (IsVerticalPathClear(column1, row1, intermediateRow) &&
                    IsHorizontalPathClear(intermediateRow, column1, column2) &&
                    IsVerticalPathClear(column2, intermediateRow, row2))
                {
                    return true;
                }
            }

            for (int intermediateCol = 0; intermediateCol < _columns; intermediateCol++)
            {
                if (IsHorizontalPathClear(row1, column1, intermediateCol) &&
                    IsVerticalPathClear(intermediateCol, row1, row2) &&
                    IsHorizontalPathClear(row2, intermediateCol, column2))
                {
                    return true;
                }
            }

            return false;
        }


        private void MatchTiles(MahjongTile tile1, MahjongTile tile2)
        {
            tile1.MarkAsMatched();
            tile2.MarkAsMatched();
        }

        public MahjongTile this[int row, int col]
        {
            get { return (MahjongTile)_board[row, col]; }
        }
    }
}

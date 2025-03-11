using UltimateMahjongConnect.Domain.Interfaces;

namespace UltimateMahjongConnect.Domain.Models
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

        public IMahjongTile[,] GetBoard() => _board;

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

        public void SetupBoardWithTilesInSpecificPattern()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 1 && j == 1 || i == 1 || j == 4)
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
                    }
                }
            }
        }

        public void SetupBoardWithTilesInSpecificPattern_SouthWest_NorthEast()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 11 && j == 3 || i == 5 || j == 12)
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
                    }
                }
            }
        }

        public void SetupBoardWithTilesInSpecificPattern_SouthEast_NorthWest()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 11 && j == 12 || i == 5 || j == 3)
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
                    }
                }
            }
        }

        public void SetupBoardWithTilesInSpecificPattern_NorthWest_SouthEast()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 5 && j == 3 || i == 11 || j == 12)
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
                    }
                }
            }
        }

        public void SetupBoardWithTilesInSpecificPattern_NorthEast_SouthWest()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 5 && j == 12 || i == 11 || j == 5)
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
                    }
                }
            }
        }

        public void SetupBoardWithTilesInRowRange()
        {
            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 1 && Enumerable.Range(1,4).Contains(j))
                    {
                        _board[i, j] = new MahjongTile(MahjongTileCategory.Bamboo, 1);
                    }
                    else
                    {
                        _board[i, j] = null;
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
            if (row1 == row2 && column1 == column2)
                return false;

            if (_board[row1, column1] is MahjongTile tile1 &&
                _board[row2, column2] is MahjongTile tile2 &&
                tile1.CanBeMatched(tile2) &&
                IsPathClear(row1, column1, row2, column2))
            {
                MatchAndRemoveTiles(tile1, tile2);
                return true;
            }

            return false;
        }

        private bool IsPathClear(int row1, int column1, int row2, int column2)
        {
            return row1 == row2 && IsHorizontalPathClear(row1, column1, column2) ||
                   column1 == column2 && IsVerticalPathClear(column1, row1, row2) ||
                   IsLShapedPathClear(row1, column1, row2, column2) ||
                   IsThreeSegmentPathClear(row1, column1, row2, column2);
        }

        public MahjongPath GetValidatedPath(int row1, int column1, int row2, int column2)
        {
            var mahjongPath = new MahjongPath();

            if(row1 == row2 && column1 == column2)
                return mahjongPath;

            if (_board[row1, column1] is MahjongTile tile1 &&
                _board[row2, column2] is MahjongTile tile2 &&
                tile1.CanBeMatched(tile2))
            {
                if (row1 == row2 && IsHorizontalPathClear(row1, column1, column2))
                {
                    mahjongPath.IsValid = true;
                    mahjongPath.PathRows.AddRange(Enumerable.Repeat(row1, Math.Abs(column2 - column1) + 1));
                    mahjongPath.PathColumns.AddRange(Enumerable.Range(Math.Min(column1, column2), Math.Abs(column2 - column1) + 1));
                    return mahjongPath;
                }

                if (column1 == column2 && IsVerticalPathClear(column1, row1, row2))
                {
                    mahjongPath.IsValid = true;
                    mahjongPath.PathRows.AddRange(Enumerable.Range(Math.Min(row1, row2), Math.Abs(row2 - row1) + 1));
                    mahjongPath.PathColumns.AddRange(Enumerable.Repeat(column1, Math.Abs(row2 - row1) + 1));
                    return mahjongPath;
                }

                if (IsLShapedPathClear(row1, column1, row2, column2))
                {
                    mahjongPath.IsValid = true;
                    mahjongPath.PathRows.AddRange(new[] { row1, row1, row2 });
                    mahjongPath.PathColumns.AddRange(new[] { column1, column2, column2 });
                    return mahjongPath;
                }

                if (IsThreeSegmentPathClear(row1, column1, row2, column2))
                {
                    mahjongPath.IsValid = true;
                    for (int intermediateRow = 0; intermediateRow < _rows; intermediateRow++)
                    {
                        if (IsVerticalPathClear(column1, row1, intermediateRow) &&
                            IsHorizontalPathClear(intermediateRow, column1, column2) &&
                            IsVerticalPathClear(column2, intermediateRow, row2))
                        {
                            mahjongPath.PathRows.AddRange(Enumerable.Range(Math.Min(row1, intermediateRow), Math.Abs(row1 - intermediateRow) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Repeat(column1, Math.Abs(row1 - intermediateRow) + 1));

                            mahjongPath.PathRows.AddRange(Enumerable.Repeat(intermediateRow, Math.Abs(column2 - column1) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Range(Math.Min(column1, column2), Math.Abs(column2 - column1) + 1));

                            mahjongPath.PathRows.AddRange(Enumerable.Range(Math.Min(intermediateRow, row2), Math.Abs(row2 - intermediateRow) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Repeat(column2, Math.Abs(row2 - intermediateRow) + 1));

                            return mahjongPath;
                        }
                    }

                    for (int intermediateCol = 0; intermediateCol < _columns; intermediateCol++)
                    {
                        if (IsHorizontalPathClear(row1, column1, intermediateCol) &&
                            IsVerticalPathClear(intermediateCol, row1, row2) &&
                            IsHorizontalPathClear(row2, intermediateCol, column2))
                        {
                            mahjongPath.PathRows.AddRange(Enumerable.Repeat(row1, Math.Abs(column1 - intermediateCol) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Range(Math.Min(column1, intermediateCol), Math.Abs(column1 - intermediateCol) + 1));

                            mahjongPath.PathRows.AddRange(Enumerable.Range(Math.Min(row1, row2), Math.Abs(row2 - row1) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Repeat(intermediateCol, Math.Abs(row2 - row1) + 1));

                            mahjongPath.PathRows.AddRange(Enumerable.Repeat(row2, Math.Abs(column2 - intermediateCol) + 1));
                            mahjongPath.PathColumns.AddRange(Enumerable.Range(Math.Min(intermediateCol, column2), Math.Abs(column2 - intermediateCol) + 1));

                            return mahjongPath;
                        }
                    }
                }
            }
            return mahjongPath;
        }


        private bool IsHorizontalPathClear(int row, int column1, int column2)
        {
            int start = Math.Min(column1, column2) + 1;
            int end = Math.Max(column1, column2);
            for (int col = start; col < end; col++)
            {
                if (_board[row, col] != null && !_board[row,col].IsRemoved)
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
            if(start <= end)
            {
                for (int row = start; row < end; row++)
                {
                    if (_board[row, column] != null && !_board[row, column].IsRemoved)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
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

        public void MatchAndRemoveTiles(IMahjongTile tile1, IMahjongTile tile2)
        {
            tile1.MarkAsMatchedAndRemoved();
            tile2.MarkAsMatchedAndRemoved();
        }

        public MahjongTile this[int row, int col]
        {
            get { return (MahjongTile)_board[row, col]; }
        }
    }
}

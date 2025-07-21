using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;

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

        public MahjongBoardData ToData()
        {
            if (_board == null)
                return new MahjongBoardData { Rows = _rows, Columns = _columns };

            var boardData = new MahjongTileData[_rows, _columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var tile = _board[i, j] as MahjongTile;
                    if (tile != null)
                    {
                        boardData[i, j] = new MahjongTileData
                        {
                            Category = (int)tile.Category,
                            Value = tile.Value,
                            IsRemoved = tile.IsRemoved,
                            IsMatched = tile.IsMatched,
                            DisplayText = tile.DisplayText
                        };
                    }
                }
            }

            return MahjongBoardData.FromMultidimensionalArray(boardData, _rows, _columns);
        }

        public void LoadFromData(MahjongBoardData boardData)
        {
            if (boardData?.Board == null)
            {
                Console.WriteLine("LoadFromData: boardData or Board is null");
                return;
            }

            Console.WriteLine($"LoadFromData: Loading board with {boardData.Rows}x{boardData.Columns}");
            _board = new MahjongTile[_rows, _columns];
            
            var multidimensionalBoard = boardData.ToMultidimensionalArray();
            
            int removedCount = 0;
            int matchedCount = 0;
            
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var tileData = multidimensionalBoard[i, j];
                    if (tileData != null)
                    {
                        var tile = new MahjongTile(
                            (MahjongTileCategory)tileData.Category,
                            tileData.Value
                        );
                        
                        tile.IsRemoved = tileData.IsRemoved;
                        tile.IsMatched = tileData.IsMatched;
                        
                        if (tileData.IsRemoved) removedCount++;
                        if (tileData.IsMatched) matchedCount++;
                        
                        _board[i, j] = tile;
                    }
                }
            }
            
            Console.WriteLine($"LoadFromData: Restored {removedCount} removed tiles and {matchedCount} matched tiles");
        }

        public void InitializeBoardPlayable()
        {
            _board = new MahjongTile[_rows, _columns];
            var tiles = _mahjongTile.GetTiles();
            
            foreach (var tile in tiles)
            {
                tile.ResetState();
            }

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 0 || i == _rows - 1 || j == 0 || j == _columns - 1)
                    {
                        _board[i, j] = new MahjongTile(true);
                    }
                }
            }

            var playableArea = (_rows - 2) * (_columns - 2);
            var tilesNeeded = Math.Min(tiles.Count, playableArea);
            
            if (tilesNeeded % 2 != 0) tilesNeeded--;
            
            var selectedTiles = new List<MahjongTile>();
            var random = new Random();
            
            for (int i = 0; i < tilesNeeded / 2; i++)
            {
                if (tiles.Count >= 2)
                {
                    var tile1 = tiles[0];
                    var matchingTile = tiles.FirstOrDefault(t => t != tile1 && t.CanBeMatched(tile1)) ?? tiles[0];
                    
                    selectedTiles.Add(tile1);
                    selectedTiles.Add(matchingTile);
                    
                    tiles.Remove(tile1);
                    if (matchingTile != tile1) tiles.Remove(matchingTile);
                }
            }
            
            selectedTiles = selectedTiles.OrderBy(x => random.Next()).ToList();
            
            PlaceTilesWithDifficulty(selectedTiles, random);
        }

        private void PlaceTilesWithDifficulty(List<MahjongTile> tiles, Random random)
        {
            var availablePositions = new List<(int row, int col)>();
            
            for (int i = 1; i < _rows - 1; i++)
            {
                for (int j = 1; j < _columns - 1; j++)
                {
                    availablePositions.Add((i, j));
                }
            }
            
            var tileIndex = 0;
            
            for (int phase = 0; phase < 3 && tileIndex < tiles.Count - 1; phase++)
            {
                if (availablePositions.Count >= 2)
                {
                    // Placer une paire avec chemin horizontal direct
                    var pos1 = availablePositions[random.Next(availablePositions.Count)];
                    availablePositions.Remove(pos1);
                    
                    // Chercher une position dans la même ligne pour un chemin facile
                    var sameRowPositions = availablePositions.Where(p => p.row == pos1.row).ToList();
                    var pos2 = sameRowPositions.Any() 
                        ? sameRowPositions[random.Next(sameRowPositions.Count)]
                        : availablePositions[random.Next(availablePositions.Count)];
                    
                    availablePositions.Remove(pos2);
                    
                    _board[pos1.row, pos1.col] = tiles[tileIndex++];
                    _board[pos2.row, pos2.col] = tiles[tileIndex++];
                }
            }
            
            while (tileIndex < tiles.Count && availablePositions.Count > 0)
            {
                var pos = availablePositions[random.Next(availablePositions.Count)];
                availablePositions.Remove(pos);
                _board[pos.row, pos.col] = tiles[tileIndex++];
            }
        }

        public void InitializeBoardWithCalculatedDifficulty()
        {
            _board = new MahjongTile[_rows, _columns];
            
            // Bordures
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (i == 0 || i == _rows - 1 || j == 0 || j == _columns - 1)
                    {
                        _board[i, j] = new MahjongTile(true);
                    }
                }
            }
            
            // Créer des patterns stratégiques
            var patterns = new[]
            {
                // Pattern 1: Paires en L
                new[] { (2, 2), (2, 5) },      // Chemin horizontal
                new[] { (3, 3), (6, 3) },      // Chemin vertical  
                new[] { (4, 7), (7, 4) },      // Chemin en L
                new[] { (5, 9), (9, 5) },      // Chemin complexe
                
                // Pattern 2: Groupes de 4 tuiles identiques
                new[] { (8, 2), (8, 6), (10, 2), (10, 6) },
                
                // Pattern 3: Paires dispersées pour chemins à 3 segments
                new[] { (2, 10), (10, 10) },
                new[] { (6, 8), (11, 2) }
            };
            
            var tiles = _mahjongTile.GetTiles().Take(60).ToList(); // Limiter pour ne pas surcharger
            var tileIndex = 0;
            var random = new Random();
            
            // Appliquer les patterns
            foreach (var pattern in patterns)
            {
                if (tileIndex >= tiles.Count - pattern.Length + 1) break;
                
                // Prendre des tuiles qui peuvent se matcher
                var patternTiles = new List<MahjongTile>();
                for (int i = 0; i < pattern.Length; i += 2)
                {
                    if (tileIndex < tiles.Count - 1)
                    {
                        // Prendre 2 tuiles identiques
                        var baseTile = tiles[tileIndex];
                        var matchingTile = tiles.Skip(tileIndex + 1)
                            .FirstOrDefault(t => t.CanBeMatched(baseTile)) ?? tiles[tileIndex + 1];
                        
                        patternTiles.Add(baseTile);
                        patternTiles.Add(matchingTile);
                        tileIndex += 2;
                    }
                }
                
                // Placer les tuiles du pattern
                for (int i = 0; i < Math.Min(pattern.Length, patternTiles.Count); i++)
                {
                    var (row, col) = pattern[i];
                    if (row < _rows && col < _columns)
                    {
                        _board[row, col] = patternTiles[i];
                    }
                }
            }
        }
    }
}

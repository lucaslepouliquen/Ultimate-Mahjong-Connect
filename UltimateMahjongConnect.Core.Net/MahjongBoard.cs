using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateMahjongConnect.Core.Net;

namespace UltimateMahjongConnect.Core.Net
{
    public class MahjongBoard
    {
        public class MahjongGame
        {
            private readonly int _rows;
            private readonly int _columns;
            private Tile[,] _board;

            public int Rows { get { return _rows; } }
            public int Columns { get { return _columns; } }

            public MahjongGame(int rows, int columns)
            {
                _rows = rows;
                _columns = columns;
                InitializeBoard();
            }

            private void InitializeBoard()
            {
                // Logic to initialize the game board with tiles
                _board = new Tile[_rows, _columns];
                // For demonstration purposes, let's fill the board with random tiles
                var tiles = Enumerable.Range(1, (_rows * _columns) / 2)
                                      .SelectMany(i => Enumerable.Repeat(i, 2))
                                      .OrderBy(_ => Guid.NewGuid())
                                      .ToArray();

                var index = 0;
                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _columns; j++)
                    {
                        _board[i, j] = new Tile(tiles[index++]);
                    }
                }
            }

            public bool CanTilesBeConnected(Tile tile1, Tile tile2)
            {
                // Logic to check if two tiles can be connected
                // For simplicity, let's assume any two tiles with the same number can be connected
                return tile1.Number == tile2.Number;
            }

            public List<(Tile, Tile)> FindMatchingTiles()
            {
                var matchingPairs = new List<(Tile, Tile)>();

                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _columns; j++)
                    {
                        var currentTile = _board[i, j];
                        if (currentTile == null || currentTile.IsMatched)
                            continue;

                        // Essayer de trouver une tuile correspondante
                        var matchingTile = FindMatchingTile(currentTile, i, j + 1, 0, 1);
                        if (matchingTile != null)
                            matchingPairs.Add((currentTile, matchingTile));

                        matchingTile = FindMatchingTile(currentTile, i, j - 1, 0, -1);
                        if (matchingTile != null)
                            matchingPairs.Add((currentTile, matchingTile));

                        matchingTile = FindMatchingTile(currentTile, i + 1, j, 1, 0);
                        if (matchingTile != null)
                            matchingPairs.Add((currentTile, matchingTile));

                        matchingTile = FindMatchingTile(currentTile, i - 1, j, -1, 0);
                        if (matchingTile != null)
                            matchingPairs.Add((currentTile, matchingTile));
                    }
                }
                return matchingPairs;
            }

            private Tile FindMatchingTile(Tile currentTile, int row, int column, int rowDelta, int columnDelta)
            {
                // Vérifiez si les coordonnées sont dans les limites du tableau
                if (row < 0 || row >= _rows || column < 0 || column >= _columns)
                    return null;

                var tileToCheck = _board[row, column];

                // Vérifiez si la tuile est valide et non appariée
                if (tileToCheck != null && !tileToCheck.IsMatched)
                {
                    // Vérifiez si les tuiles peuvent être connectées
                    if (CanTilesBeConnected(currentTile, tileToCheck))
                    {
                        return tileToCheck;
                    }
                }

                // Si les tuiles ne peuvent pas être connectées, continuez la recherche
                return FindMatchingTile(currentTile, row + rowDelta, column + columnDelta, rowDelta, columnDelta);
            }

        }
    }
}

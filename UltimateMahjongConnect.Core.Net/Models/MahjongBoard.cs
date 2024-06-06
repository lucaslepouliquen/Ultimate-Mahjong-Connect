using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongBoard : IMahjongBoard
    {

        private readonly int _rows;
        private readonly int _columns;
        private readonly IMahjongTile _mahjongTile;
        private MahjongTile[,] _board;

        public int Rows { get { return _rows; } }
        public int Columns { get { return _columns; } }
        public MahjongBoard(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            var tiles = _mahjongTile.GetTiles();
            var random = new Random();

            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var randomTileIndex = random.Next(tiles.Count);
                    var randomTile = tiles[randomTileIndex];

                    _board[i, j] = new MahjongTile(i, j, randomTile.Image);
                    tiles.RemoveAt(randomTileIndex);
                }
            }
        }

        public bool CanTilesConnect(MahjongTile tile1, MahjongTile tile2)
        {
            if (tile1.Image != tile2.Image || tile1.IsRemoved || tile2.IsRemoved)
            {
                return false;
            }

            if (AreDirectlyConnected(tile1, tile2)) return true;
            if (HasOneBendPath(tile1, tile2)) return true;
            if (HasTwoBendsPath(tile1, tile2)) return true;

            return false;
        }

        private bool AreDirectlyConnected(MahjongTile tile1, MahjongTile tile2)
        {
            if (tile1.Y == tile2.Y)
            {
                int minX = Math.Min(tile1.X, tile2.X);
                int maxX = Math.Max(tile1.X, tile2.X);
                for (int x = minX + 1; x < maxX; x++)
                {
                    if (!_board[x, tile1.Y].IsRemoved) return false;
                }
                return true;
            }

            if (tile1.X == tile2.X)
            {
                int minY = Math.Min(tile1.Y, tile2.Y);
                int maxY = Math.Max(tile1.Y, tile2.Y);
                for (int y = minY + 1; y < maxY; y++)
                {
                    if (!_board[tile1.X, y].IsRemoved) return false;
                }
                return true;
            }

            return false;
        }

        private bool HasOneBendPath(MahjongTile tile1, MahjongTile tile2)
        {
            if (IsPathClear(tile1.X, tile2.X, tile1.Y) && IsPathClear(tile2.Y, tile1.Y, tile2.X))
            {
                return true;
            }

            if (IsPathClear(tile1.Y, tile2.Y, tile1.X) && IsPathClear(tile2.X, tile1.X, tile2.Y))
            {
                return true;
            }
            return false;
        }

        private bool HasTwoBendsPath(MahjongTile tile1, MahjongTile tile2)
        {
            for (int x = 0; x < _rows; x++)
            {
                if (IsPathClear(tile1.X, x, tile1.Y) && IsPathClear(tile1.Y, tile2.Y, x) && IsPathClear(x, tile2.X, tile2.Y))
                {
                    return true;
                }
            }
            for (int y = 0; y < _columns; y++)
            {
                if (IsPathClear(tile1.Y, y, tile1.X) && IsPathClear(tile1.X, tile2.X, y) && IsPathClear(y, tile2.Y, tile2.X))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPathClear(int start, int end, int constant)
        {
            int min = Math.Min(start, end);
            int max = Math.Max(start, end);

            for (int i = min + 1; i < max; i++)
            {
                if (_board[i, constant].IsRemoved == false)
                {
                    return false;
                }
            }
            return true;
        }

        public List<(MahjongTile, MahjongTile)> FindMatchingTiles()
        {
            var matchingPairs = new List<(MahjongTile, MahjongTile)>();

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

        private MahjongTile FindMatchingTile(MahjongTile currentTile, int row, int column, int rowDelta, int columnDelta)
        {
            if (row < 0 || row >= _rows || column < 0 || column >= _columns)
                return null;

            var tileToCheck = _board[row, column];

            if (tileToCheck != null && !tileToCheck.IsMatched)
            {
                if (CanTilesConnect(currentTile, tileToCheck))
                {
                    return tileToCheck;
                }
            }

            return FindMatchingTile(currentTile, row + rowDelta, column + columnDelta, rowDelta, columnDelta);
        }
    }
}

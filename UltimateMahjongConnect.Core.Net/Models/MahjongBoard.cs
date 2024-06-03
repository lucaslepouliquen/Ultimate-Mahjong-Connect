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
        private Tile[,] _board;

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
            // Liste des images disponibles pour les tuiles
            var tileImages = new List<string>
{
    // Bambous (Bamboo)
    "bamboo1.jpg",
    "bamboo2.jpg",
    "bamboo3.jpg",
    "bamboo4.jpg",
    "bamboo5.jpg",
    "bamboo6.jpg",
    "bamboo7.jpg",
    "bamboo8.jpg",
    "bamboo9.jpg",

    // Cercles (Circles)
    "circle1.jpg",
    "circle2.jpg",
    "circle3.jpg",
    "circle4.jpg",
    "circle5.jpg",
    "circle6.jpg",
    "circle7.jpg",
    "circle8.jpg",
    "circle9.jpg",

    // Caractères (Characters)
    "character1.jpg",
    "character2.jpg",
    "character3.jpg",
    "character4.jpg",
    "character5.jpg",
    "character6.jpg",
    "character7.jpg",
    "character8.jpg",
    "character9.jpg",

    // Vents (Winds)
    "east.jpg",
    "west.jpg",
    "north.jpg",
    "south.jpg",

    // Dragons (Dragons)
    "red_dragon.jpg",
    "green_dragon.jpg",
    "white_dragon.jpg",

    // Fleurs (Flowers)
    "plum_flower.jpg",
    "orchid.jpg",
    "chrysanthemum.jpg",
    "bamboo_flower.jpg",

    // Saisons (Seasons)
    "spring.jpg",
    "summer.jpg",
    "autumn.jpg",
    "winter.jpg"
};


            _board = new Tile[_rows, _columns];

            // Remplir le tableau avec des tuiles aléatoires
            var random = new Random();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    // Choisir une image aléatoire pour la tuile
                    var randomImageIndex = random.Next(tileImages.Count);
                    var randomImage = tileImages[randomImageIndex];

                    _board[i, j] = new Tile(i, j, randomImage);
                }
            }
        }

        public bool CanTilesConnect(Tile tile1, Tile tile2)
        {
            if (tile1.Image != tile2.Image || tile1.IsRemoved || tile2.IsRemoved)
            {
                return false;
            }

            // Check direct horizontal and vertical connections
            if (AreDirectlyConnected(tile1, tile2)) return true;

            // Check for one bend connection (L shape)
            if (HasOneBendPath(tile1, tile2)) return true;

            // Check for two bends connection (U or Z shape)
            if (HasTwoBendsPath(tile1, tile2)) return true;

            return false;
        }

        private bool AreDirectlyConnected(Tile tile1, Tile tile2)
        {
            // Horizontal connection
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

            // Vertical connection
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

        private bool HasOneBendPath(Tile tile1, Tile tile2)
        {
            // L shape connection
            // Check if there is an L shape connection via tile1.X, tile2.Y
            if (IsPathClear(tile1.X, tile2.X, tile1.Y) && IsPathClear(tile2.Y, tile1.Y, tile2.X))
            {
                return true;
            }
            // Check if there is an L shape connection via tile2.X, tile1.Y
            if (IsPathClear(tile1.Y, tile2.Y, tile1.X) && IsPathClear(tile2.X, tile1.X, tile2.Y))
            {
                return true;
            }
            return false;
        }

        private bool HasTwoBendsPath(Tile tile1, Tile tile2)
        {
            // Check U or Z shape connection
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
                if (CanTilesConnect(currentTile, tileToCheck))
                {
                    return tileToCheck;
                }
            }

            // Si les tuiles ne peuvent pas être connectées, continuez la recherche
            return FindMatchingTile(currentTile, row + rowDelta, column + columnDelta, rowDelta, columnDelta);
        }
    }
}

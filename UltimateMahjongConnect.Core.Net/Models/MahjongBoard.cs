using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongBoard : IMahjongBoard
    {

        private readonly int _rows = 12;
        private readonly int _columns = 12;
        private readonly IMahjongTile _mahjongTile;
        private IMahjongTile[,] _board;
        public MahjongBoard(IMahjongTile mahjongTile)
        {
            _mahjongTile = mahjongTile;
        }

        public void InitializeBoardDeterministically()
        {
            InitializeBoard(randomize: false);
        }


        public void InitializeBoardRandom()
        {
            InitializeBoard(randomize:true);
        }

        private void InitializeBoard(bool randomize)
        {
            var tiles = _mahjongTile.GetTiles();
            var random = new Random();

            _board = new MahjongTile[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (tiles.Count == 0)
                    {
                        throw new InvalidOperationException("Not enough tiles to fill the board");
                    }
                    int tileIndex = randomize ? random.Next(tiles.Count) : 0;
                    var randomTile = tiles[tileIndex];

                    _board[i, j] = randomTile;
                    tiles.RemoveAt(tileIndex);
                }
            }
        }

        public bool IsPathValid(int row1, int column1, int row2, int column2)
        {
            return true;
        }

        public MahjongTile this[int row, int col]
        {
            get { return (MahjongTile)_board[row, col]; }
        }
    }
}

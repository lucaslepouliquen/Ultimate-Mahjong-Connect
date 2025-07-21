namespace UltimateMahjongConnect.Domain.Models
{
    public class MahjongBoardData
    {
        public MahjongTileData[][] Board { get; set; } = new MahjongTileData[0][];
        public int Rows { get; set; }
        public int Columns { get; set; }
        
        public MahjongTileData[,] ToMultidimensionalArray()
        {
            if (Board == null || Board.Length == 0) 
                return new MahjongTileData[Rows, Columns];

            var result = new MahjongTileData[Rows, Columns];
            for (int i = 0; i < Rows && i < Board.Length; i++)
            {
                if (Board[i] != null)
                {
                    for (int j = 0; j < Columns && j < Board[i].Length; j++)
                    {
                        result[i, j] = Board[i][j];
                    }
                }
            }
            return result;
        }
        
        public static MahjongBoardData FromMultidimensionalArray(MahjongTileData[,] board, int rows, int columns)
        {
            var result = new MahjongBoardData
            {
                Rows = rows,
                Columns = columns,
                Board = new MahjongTileData[rows][]
            };
            
            for (int i = 0; i < rows; i++)
            {
                result.Board[i] = new MahjongTileData[columns];
                for (int j = 0; j < columns; j++)
                {
                    result.Board[i][j] = board[i, j];
                }
            }
            
            return result;
        }
    }

    public class MahjongTileData
    {
        public int Category { get; set; }
        public int Value { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsMatched { get; set; }
        public string DisplayText { get; set; } = "";
    }
} 
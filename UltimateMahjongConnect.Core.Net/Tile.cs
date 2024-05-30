namespace UltimateMahjongConnect.Core.Net
{
    public class Tile
    {
        public int Number { get; }
        public bool IsMatched { get; set; }

        public Tile(int number)
        {
            Number = number;
            IsMatched = false;
        }
    }
}

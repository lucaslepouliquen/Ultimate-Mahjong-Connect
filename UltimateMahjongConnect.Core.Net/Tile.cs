using static System.Net.Mime.MediaTypeNames;

namespace UltimateMahjongConnect.Core.Net
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsMatched { get; set; }

        public Tile(int x, int y, string image)
        {
            X = x;
            Y = y;
            Image = image;
            IsRemoved = false;
            IsMatched = false; 
        }
    }
}

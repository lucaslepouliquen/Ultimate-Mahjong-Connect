using UltimateMahjongConnect.Core.Net.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongTile : IMahjongTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsMatched { get; set; }

        public MahjongTile(int x, int y, string image)
        {
            X = x;
            Y = y;
            Image = image;
            IsRemoved = false;
            IsMatched = false;
        }

        public List<MahjongTile> GetTiles()
        {
            var tiles = new List<MahjongTile>();

            var tileImageMappings = new List<(MahjongTileCategory Category, string[] Images)>
            {
                (MahjongTileCategory.Bamboo, new string[] { "bamboo1.jpg", "bamboo2.jpg", "bamboo3.jpg", "bamboo4.jpg", "bamboo5.jpg", "bamboo6.jpg", "bamboo7.jpg", "bamboo8.jpg", "bamboo9.jpg" }),
                (MahjongTileCategory.Circles, new string[] { "circle1.jpg", "circle2.jpg", "circle3.jpg", "circle4.jpg", "circle5.jpg", "circle6.jpg", "circle7.jpg", "circle8.jpg", "circle9.jpg" }),
                (MahjongTileCategory.Characters, new string[] { "character1.jpg", "character2.jpg", "character3.jpg", "character4.jpg", "character5.jpg", "character6.jpg", "character7.jpg", "character8.jpg", "character9.jpg" }),
                (MahjongTileCategory.Winds, new string[] { "east.jpg", "west.jpg", "north.jpg", "south.jpg" }),
                (MahjongTileCategory.Dragons, new string[] { "red_dragon.jpg", "green_dragon.jpg", "white_dragon.jpg" }),
                (MahjongTileCategory.Flowers, new string[] { "plum_flower.jpg", "orchid.jpg", "chrysanthemum.jpg", "bamboo_flower.jpg" }),
                (MahjongTileCategory.Seasons, new string[] { "spring.jpg", "summer.jpg", "autumn.jpg", "winter.jpg" })
            };

            foreach (var mapping in tileImageMappings)
            {
                var category = mapping.Category;
                var images = mapping.Images;
                for (int i = 0; i < images.Length; i++)
                {
                    tiles.Add(new MahjongTile(i, (int)category, images[i]));
                }
            }

            return tiles;
        }
    }
}

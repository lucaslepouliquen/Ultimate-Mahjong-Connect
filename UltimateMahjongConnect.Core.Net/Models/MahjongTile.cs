using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongTile : IMahjongTile
    {
        private MahjongTileCategory Category;
        private string Image;
        private bool IsRemoved;
        private bool IsMatched;

        public MahjongTile()
        {
            IsRemoved = false;
            IsMatched = false;
        }

        public MahjongTile(MahjongTileCategory category, string image) : this()
        {
            Category = category;
            Image = image;
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

            foreach (var (category, images) in tileImageMappings)
            {
                int repetitions = GetRepetitions(category);
                foreach (var image in images)
                {
                    for (int i = 0; i < repetitions; i++)
                    {
                        tiles.Add(new MahjongTile(category, image));
                    }
                }
            }
            return tiles;
        }

        private int GetRepetitions(MahjongTileCategory category)
        {
            var fieldInfo = category.GetType().GetField(category.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(TileRepetitionAttribute), false);
            return attributes.Length > 0 ? ((TileRepetitionAttribute)attributes[0]).Repetitions : 1;
        }
    }
}

using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongTile : IMahjongTile
    {
        private MahjongTileCategory Category;
        private int Value;
        private bool IsRemoved;
        private bool IsMatched;

        public MahjongTile()
        {
            IsRemoved = false;
            IsMatched = false;
        }

        public MahjongTile(MahjongTileCategory category, int value) : this()
        {
            Category = category;
            Value = value;
        }

        public List<MahjongTile> GetTiles()
        {
            var tiles = new List<MahjongTile>();
            var tileCategories = Enum.GetValues(typeof(MahjongTileCategory));

            foreach (MahjongTileCategory category in tileCategories)
            {
                int repetitions = GetRepetitions(category);
                int maxValue = category switch
                {
                    MahjongTileCategory.Bamboo or MahjongTileCategory.Circles or MahjongTileCategory.Characters => 8,
                    MahjongTileCategory.Winds => 3,
                    MahjongTileCategory.Dragons => 2,
                    MahjongTileCategory.Flowers or MahjongTileCategory.Seasons => 3,
                    _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Unknown Mahjong tile category")
                };

                for (int value = 0; value <= maxValue; value++)
                {
                    for (int i = 0; i < repetitions; i++)
                    {
                        var tileValue = (category == MahjongTileCategory.Flowers || category == MahjongTileCategory.Seasons) ? 1 : value;
                        tiles.Add(new MahjongTile(category, tileValue));
                    }
                }
            }

            return tiles;
        }


        private int GetRepetitions(MahjongTileCategory category)
        {
            var fieldInfo = category.GetType().GetField(category.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(TileRepetitionAttribute), false);
            var repetitions = attributes.Length > 0 ? ((TileRepetitionAttribute)attributes[0]).Repetitions : 1;
            return repetitions;
        }
    }
}

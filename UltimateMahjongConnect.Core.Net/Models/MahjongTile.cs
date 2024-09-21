using UltimateMahjongConnect.Core.Net.Interfaces;

namespace UltimateMahjongConnect.Core.Net.Models
{
    public class MahjongTile : IMahjongTile
    {
        private readonly MahjongTileCategory _category;
        private int _value;
        private bool _isRemoved;
        private bool _isMatched;
        public MahjongTileCategory Category => _category;
        public int Value => _value;

        public bool IsRemoved => _isRemoved;
        public bool IsMatched => _isMatched;

        public MahjongTile()
        {
            _isRemoved = false;
            _isMatched = false;
        }

        public MahjongTile(MahjongTileCategory category, int value) : this()
        {
            _category = category;
            _value = value;
        }

        public List<MahjongTile> GetTiles()
        {
            var tiles = new List<MahjongTile>();
            var tileCategories = Enum.GetValues(typeof(MahjongTileCategory));

            foreach (MahjongTileCategory category in tileCategories)
            {
                int repetitions = GetRepetitions(category);
                int maxValue = GetMaxValue(category);
                tiles.AddRange(CreateTilesForCategory(category, repetitions, maxValue));
            }

            return tiles;
        }

        private IEnumerable<MahjongTile> CreateTilesForCategory(MahjongTileCategory category, int repetitions, int maxValue)
        {
            for (int value = 0; value <= maxValue; value++)
            {
                for (int i = 0; i < repetitions; i++)
                {
                    var tileValue = (category == MahjongTileCategory.Flowers || category == MahjongTileCategory.Seasons) ? 1 : value;
                    yield return new MahjongTile(category, tileValue);
                }
            }
        }

        private static int GetMaxValue(MahjongTileCategory category)
        {
            return category switch
            {
                MahjongTileCategory.Bamboo or MahjongTileCategory.Circles or MahjongTileCategory.Characters => 8,
                MahjongTileCategory.Winds => 3,
                MahjongTileCategory.Dragons => 2,
                MahjongTileCategory.Flowers or MahjongTileCategory.Seasons => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Unknown Mahjong tile category")
            };
        }

        private int GetRepetitions(MahjongTileCategory category)
        {
            var fieldInfo = category.GetType().GetField(category.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(TileRepetitionAttribute), false);
            var repetitions = attributes.Length > 0 ? ((TileRepetitionAttribute)attributes[0]).Repetitions : 1;
            return repetitions;
        }

        public bool CanBeMatched(MahjongTile otherTile)
        {
            if (otherTile == null || this._isRemoved || otherTile._isRemoved)
                return false;

            if(otherTile is { _category: var category, _value: var value } && _category == category && _value == value)
            {
                _isMatched = true;
                otherTile._isMatched = true;
                _isRemoved = true;
                otherTile._isRemoved = true;
                return true;
            }

            return false;
        }
    }
}

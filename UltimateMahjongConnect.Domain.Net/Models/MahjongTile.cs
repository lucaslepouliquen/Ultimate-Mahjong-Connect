using UltimateMahjongConnect.Domain.Interfaces;

namespace UltimateMahjongConnect.Domain.Models
{
    public class MahjongTile : IMahjongTile
    {
        public event Action TileChanged;
        private readonly MahjongTileCategory _category;
        private int _value;
        private bool _isRemoved;
        private bool _isMatched;
        public MahjongTileCategory Category => _category;
        public int Value => _value;
        public bool IsRemoved {
            get => _isRemoved;
            set
            {
                _isRemoved = value;
                TileChanged?.Invoke();
            }
        }
        public bool IsMatched { 
            get => _isMatched;
            set
            {
                _isMatched = value;
                TileChanged?.Invoke();
            }
        }

        public MahjongTile()
        {
            _isRemoved = false;
            _isMatched = false;
        }

        public MahjongTile(bool isRemoved)
        {
            _isRemoved = isRemoved;
            _isMatched = false;
        }

        public MahjongTile(MahjongTileCategory category, int value) : this()
        {
            _category = category;
            _value = value;
        }
        public string DisplayText => _isRemoved ? string.Empty : $"[{Category.ToString().Substring(0, 3)}-{Value}]";

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
                    var tileValue = category == MahjongTileCategory.Flowers || category == MahjongTileCategory.Seasons ? 1 : value;
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
            if (otherTile == null || _isRemoved || otherTile._isRemoved)
                return false;
            var IsMatched = otherTile is { _category: var category, _value: var value } && _category == category && _value == value;
            return IsMatched;
        }

        public void ResetState()
        {
            _isRemoved = false;
            _isMatched = false;
        }


        public override string ToString()
        {
            if (_isRemoved)
            {
                return string.Empty;
            }
            return $"[{Category.ToString().Substring(0, 3)}-{Value}]";
        }

        public void MarkAsMatchedAndRemoved()
        {
            _isMatched = true;
            _isRemoved = true;
            IsRemoved = true;
        }
    }
}

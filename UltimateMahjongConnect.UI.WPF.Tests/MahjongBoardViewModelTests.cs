using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF.Tests
{
    public class MahjongBoardViewModelTests
    {
        private MahjongBoard _mahjongBoard;
        private readonly Gamer _gamer;
        private readonly MahjongBoardViewModel _viewModel;

        public MahjongBoardViewModelTests()
        {
            _mahjongBoard = new MahjongBoard(new MahjongTile());
            _gamer = new Gamer();
            _viewModel = new MahjongBoardViewModel(_mahjongBoard, _gamer);
        }

        private void InitializeBoardDeterministically()
        {
            _mahjongBoard.InitializeBoardDeterministically();
        }

        private async Task ExecuteTileCommand(MahjongTileViewModel tileViewModel)
        {
            if (_viewModel.TileCommand.CanExecute(tileViewModel))
            {
                await ((AsyncRelayCommand<MahjongTileViewModel>)_viewModel.TileCommand).ExecuteAsync(tileViewModel);
            }
        }

        private MahjongTileViewModel CreateTileViewModel(MahjongTileCategory category, int value, int row, int column)
        {
            var tile = new MahjongTile(category, value);
            return new MahjongTileViewModel(tile, row, column);
        }

        [Fact]
        public async Task TileCommand_ShouldNotUpdateScore_WhenTileGotSameRowAndColumn()
        {
            InitializeBoardDeterministically();

            var tileViewModel1 = CreateTileViewModel(MahjongTileCategory.Bamboo, 1, 1, 1);
            var tileViewModel2 = CreateTileViewModel(MahjongTileCategory.Bamboo, 2, 1, 1);

            await ExecuteTileCommand(tileViewModel1);
            await ExecuteTileCommand(tileViewModel2);

            Assert.Equal(0, _viewModel.Score);
        }

        [Fact]
        public async Task TileCommand_ShouldUpdateScore_WhenPathIsValid_AdjacentTiles()
        {
            InitializeBoardDeterministically();

            var tileViewModel1 = CreateTileViewModel(MahjongTileCategory.Bamboo, 1, 1, 1);
            var tileViewModel2 = CreateTileViewModel(MahjongTileCategory.Bamboo, 2, 1, 2);

            await ExecuteTileCommand(tileViewModel1);
            await ExecuteTileCommand(tileViewModel2);

            Assert.Equal(10, _viewModel.Score);
        }

        [Theory]
        [InlineData(1,1,1,3)]
        [InlineData(1,1,1,4)]
        public async Task TileCommand_ShouldUpdateScore_WhenPathIsValid_NonAdjacentTiles(int selectedRowTile1, int selectedColumnTile1, int selectedRowTile2, int selectedColumnTile2)
        {
            InitializeBoardDeterministically();

            var tileViewModel1 = CreateTileViewModel(MahjongTileCategory.Bamboo, 1, selectedRowTile1, selectedColumnTile1);
            var tileViewModel2 = CreateTileViewModel(MahjongTileCategory.Bamboo, 2, selectedRowTile2, selectedColumnTile2);

            await ExecuteTileCommand(tileViewModel1);
            await ExecuteTileCommand(tileViewModel2);

            Assert.Equal(10, _viewModel.Score);
        }
    }
}

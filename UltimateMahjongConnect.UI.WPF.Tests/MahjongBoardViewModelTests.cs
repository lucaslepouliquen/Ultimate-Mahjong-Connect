using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateMahjongConnect.Business.Interfaces;
using UltimateMahjongConnect.Business.Models;
using UltimateMahjongConnect.Core.Net.Interfaces;
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

        [Fact]
        public async Task TileCommand_ShouldUpdateScore_WhenPathIsValid()
        {
            InitializeBoardDeterministically();

            var tile1 = new MahjongTile(MahjongTileCategory.Bamboo, 1);
            var tile2 = new MahjongTile(MahjongTileCategory.Bamboo, 2);

            int row1 = 1; int column1 = 1;
            int row2 = 1; int column2 = 2;
            var tileViewModel1 = new MahjongTileViewModel(tile1, row1, column1);
            var tileViewModel2 = new MahjongTileViewModel(tile2, row2, column2);

            if (_viewModel.TileCommand.CanExecute(tileViewModel1))
            {
                await ((AsyncRelayCommand<MahjongTileViewModel>)_viewModel.TileCommand).ExecuteAsync(tileViewModel1);
            }

            if (_viewModel.TileCommand.CanExecute(tileViewModel2))
            {
                await ((AsyncRelayCommand<MahjongTileViewModel>)_viewModel.TileCommand).ExecuteAsync(tileViewModel2);
            }

            Assert.True(_viewModel.Score.Equals(10));
        }

    }
}

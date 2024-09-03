using AutoMapper;
using Moq;
using System.Collections.ObjectModel;
using UltimateMahjongConnect.Service.Interface;
using UltimateMahjongConnect.UI.WPF.ViewModel;
using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.UI.WPF.Model;
using UltimateMahjongConnect.Service.Profiles;
using UltimateMahjongConnect.UI.WPF.Profiles;
using UltimateMahjongConnect.UI.WPF.Tests;

namespace UltimateMahjongConnect.Test
{
    public class GamersViewModelTests
    {
        private Mock<IGamerService>? _gamerServiceMock;
        private IMapper _mapper;
        private GamersViewModel _gamersViewModel;

        public GamersViewModelTests()
        {
            _gamerServiceMock = new Mock<IGamerService>();
            _mapper = AutoMapperConfigForTests.CreateMapper();
            _gamersViewModel = new GamersViewModel(_gamerServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task LoadGamerAsync_ShouldSetGamers()
        {
            // Arrange
            var gamersEntity = new List<GamerEntity>
                        {
                            new GamerEntity { Id = 1, Pseudonyme = "Gamer1" },
                            new GamerEntity { Id = 2, Pseudonyme = "Gamer2" }
                        };
            var gamers = new ObservableCollection<GamerItemViewModel>
                        {
                            new GamerItemViewModel(new GamerModel { Id = 1, Pseudonyme = "Gamer1" }),
                            new GamerItemViewModel(new GamerModel { Id = 2, Pseudonyme = "Gamer2" })
                        };
            _gamerServiceMock.Setup(x => x.GetAllGamerAsync()).ReturnsAsync(gamersEntity);

            // Act
            await _gamersViewModel.LoadGamerAsync();

            // Assert
            Assert.Equal(gamers.Select(g => g.Id), _gamersViewModel.Gamers.Select(g => g.Id));
        }

        [Fact]
        public void MoveNavigation_ShouldChangeNavigationSide()
        {
            // Arrange
            _gamersViewModel.NavigationSide = NavigationSide.Left;

            // Act
            _gamersViewModel.MoveNavigation(null);

            // Assert
            Assert.Equal(NavigationSide.Right, _gamersViewModel.NavigationSide);
        }
    }
}

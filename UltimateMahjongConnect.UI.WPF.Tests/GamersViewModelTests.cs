using AutoMapper;
using Moq;
using System.Collections.ObjectModel;
using UltimateMahjongConnect.UI.WPF.ViewModel;
using UltimateMahjongConnect.UI.WPF.Model;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Infrastructure.Persistence;

namespace UltimateMahjongConnect.UI.WPF.Tests
{
    public class GamersViewModelTests
    {
        private Mock<IGamerRepository>? _gamerServiceMock;
        private IMapper _mapper;
        private GamersViewModel _gamersViewModel;

        public GamersViewModelTests()
        {
            _gamerServiceMock = new Mock<IGamerRepository>();
            _mapper = AutoMapperConfigForTests.CreateMapper();
            _gamersViewModel = new GamersViewModel(_gamerServiceMock.Object, _mapper);
        }

        public static IEnumerable<object[]> GetGamerEntityAndViewModelsTestData()
        {
            yield return new object[]
            {
                new List<Gamer>(),
                new List<GamerModel>() 
            };

            yield return new object[]
            {
                new List<Gamer>
                {
                    new Gamer("Joueur1", 25, "joueur1@example.com"),
                    new Gamer("Joueur2", 30, "joueur2@example.com")
                },
                new List<GamerModel>
                {
                    new GamerModel { Id = 1, Pseudonyme = "Gamer1" },
                    new GamerModel { Id = 2, Pseudonyme = "Gamer2" }
                }
            };
        }

        [Theory]
        [MemberData(nameof(GetGamerEntityAndViewModelsTestData))]
        public async Task LoadGamerAsync_ShouldLoadCorrectGamers(List<Gamer> gamers, List<GamerModel> expectedGamers)
        {
            // Arrange
            _gamerServiceMock.Setup(x => x.GetAllGamerAsync()).ReturnsAsync(gamers);

            // Act
            await _gamersViewModel.LoadGamerAsync();

            // Assert
            Assert.Equal(expectedGamers.Select(g => g.Id), _gamersViewModel.Gamers.Select(g => g.Id));
            Assert.Equal(expectedGamers.Select(g => g.Pseudonyme), _gamersViewModel.Gamers.Select(g => g.Pseudonyme));
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

        [Fact]
        public void Delete_ShouldDeleteGamer()
        {
            //Arrange
            var gamer1 = new GamerItemViewModel(new GamerModel { Id = 1, Pseudonyme = "Gamer1" });
            var gamer2 = new GamerItemViewModel(new GamerModel { Id = 2, Pseudonyme = "Gamer2" });

            _gamersViewModel.GamersList = new ObservableCollection<GamerItemViewModel>
            {
                gamer1
                , gamer2
            };
            _gamersViewModel.SelectedGamer = gamer1;

            //Act
            _gamersViewModel.DeleteCommand.Execute(null);

            //Assert
            Assert.Contains(gamer2, _gamersViewModel.GamersList);
            Assert.DoesNotContain(gamer1, _gamersViewModel.GamersList);
            Assert.Null(_gamersViewModel.SelectedGamer);
        }
    }
}
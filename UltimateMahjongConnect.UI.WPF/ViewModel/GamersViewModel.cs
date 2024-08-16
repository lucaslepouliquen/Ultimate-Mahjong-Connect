using AutoMapper;
using UltimateMahjongConnect.Service.Services;
using UltimateMahjongConnect.UI.WPF.Data;
using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class GamersViewModel : ValidationViewModelBase
    {
        private readonly IGamerDataProvider _gamerDataProvider;
        private readonly GamerService _gamerService;
        private readonly IMapper _mapper;
        private GamerItemViewModel? _selectedGamer;
        private NavigationSide _navigationSide;

        public GamersViewModel(IGamerDataProvider gamerDataProvider)
        {
            _gamerDataProvider = gamerDataProvider; 
        }

        private List<GamerModel>? _gamers;
        public List<GamerModel>? Gamers
        {
            get => _gamers;
            set
            {
                _gamers = value;
                RaisePropertyChanged();
            }
        }

        public NavigationSide NavigationSide
        {
            get => _navigationSide;
            set
            {
                _navigationSide = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadGamerAsync()
        {
            var gamersDto = await _gamerService.GetAllGamerAsync();
            Gamers = _mapper.Map<List<GamerModel>>(gamersDto);
        }


        private void MoveNavigation(object? parameter)
        {
            NavigationSide = NavigationSide == NavigationSide.Left
                ? NavigationSide.Right
                : NavigationSide.Left;
        }   
    }

    public enum NavigationSide
    {
        Left,
        Right
    }
}

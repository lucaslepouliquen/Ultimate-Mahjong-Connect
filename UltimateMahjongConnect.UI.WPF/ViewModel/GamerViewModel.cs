using AutoMapper;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Service.Services;
using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class GamerViewModel : ValidationViewModelBase
    {
        private readonly GamerModel _gamerModel;
        private readonly GamerService _gamerService;
        private readonly IMapper _mapper;

        public GamerViewModel(GamerModel gamerModel)
        {
            _gamerModel = gamerModel; 
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

        public string? Pseudonyme
        {
            get => _gamerModel.Pseudonyme;
            set
            {
                _gamerModel.Pseudonyme = value;
                RaisePropertyChanged();
                if(string.IsNullOrEmpty(_gamerModel.Pseudonyme))
                {
                    AddError("Pseudonyme", "Pseudonyme cannot be empty");
                }
                else
                {
                    ClearErrors("Pseudonyme");
                }
            }
        }
        
        public string? Password
        {
            get => _gamerModel.Password; 
            set
            {
                _gamerModel.Password = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(_gamerModel.Password))
                {
                    AddError("Password", "Password cannot be empty");
                }
                else
                {
                    ClearErrors("Password");
                }
            }
        }

        public async Task LoadGamerAsync()
        {
            var gamersDto = await _gamerService.GetAllGamerAsync();
            Gamers = _mapper.Map<List<GamerModel>>(gamersDto);
        }
    }
}

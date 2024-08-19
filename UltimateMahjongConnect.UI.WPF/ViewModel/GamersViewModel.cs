﻿using AutoMapper;
using System.Collections.ObjectModel;
using UltimateMahjongConnect.Service.Interface;
using UltimateMahjongConnect.UI.WPF.Model;

namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class GamersViewModel : ValidationViewModelBase
    {
        private readonly IGamerService _gamerService;
        private readonly IMapper _mapper;
        private GamerItemViewModel? _selectedGamer;
        private NavigationSide _navigationSide;

        public GamersViewModel(IGamerService gamerService)
        {
            _gamerService = gamerService;
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
        }
        public DelegateCommand DeleteCommand { get; }
        public ObservableCollection<GamerItemViewModel>? Gamers { get; set; } = new();
        public ObservableCollection<GamerItemViewModel>? GamersList { get; set; }

        public GamerItemViewModel? SelectedGamer
        {
            get => _selectedGamer;
            set
            {
                _selectedGamer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsGamerSelected));
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsGamerSelected => SelectedGamer is not null;
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
            Gamers = _mapper.Map<ObservableCollection<GamerItemViewModel>>(gamersDto);
        }


        private void MoveNavigation(object? parameter)
        {
            NavigationSide = NavigationSide == NavigationSide.Left
                ? NavigationSide.Right
                : NavigationSide.Left;
        }

        private void Delete(object? obj)
        {
            if(SelectedGamer is not null) 
            {
                GamersList?.Remove(SelectedGamer);
                SelectedGamer = null;
            }
        }
        private bool CanDelete(object? parameter) => SelectedGamer is not null;

        public async override Task LoadAsync()
        {
            if (Gamers.Any())
            {
                return;
            }

            var gamersDto = await _gamerService.GetAllGamerAsync();
            var gamers = _mapper.Map<List<GamerModel>>(gamersDto);

            if (gamers is not null)
            {
                foreach (var gamer in gamers)
                {
                    Gamers.Add(new GamerItemViewModel(gamer));
                }
            }
        }
    }
}

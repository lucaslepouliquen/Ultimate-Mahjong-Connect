namespace UltimateMahjongConnect.UI.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;
        public MainViewModel(GamersViewModel gamersViewModel, MahjongBoardViewModel mahjongBoardViewModel) 
        {
            GamersViewModel = gamersViewModel;
            MahjongBoardViewModel = mahjongBoardViewModel;
            SelectedViewModel = mahjongBoardViewModel;
            SelectViewModelCommand = new DelegateCommand(SelectViewModel);

        }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel; 

            set
            {
                _selectedViewModel = value;
                RaisePropertyChanged();
            }
        }

        public GamersViewModel GamersViewModel { get; }
        public MahjongBoardViewModel MahjongBoardViewModel { get; }
        public DelegateCommand SelectViewModelCommand { get; }
        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();
        }
    }
}

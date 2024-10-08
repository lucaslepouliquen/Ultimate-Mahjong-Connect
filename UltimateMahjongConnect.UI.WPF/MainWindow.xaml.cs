using System.Windows;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }

        private void InitializeRandomBoard_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedViewModel is MahjongBoardViewModel mahjongViewModel)
            {
                mahjongViewModel.InitializeRandomCommand.Execute(null);
            }
        }
    }
}
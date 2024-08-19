using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.Service;
using UltimateMahjongConnect.Service.Interface;
using UltimateMahjongConnect.Service.Services;
using UltimateMahjongConnect.UI.WPF.Data;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbSQLContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UltimateMahjongConnect;Trusted_Connection=True;MultipleActiveResultSets=true"));
            AutoMapperConfig.RegisterMappings(services);
            services.AddTransient<IGamerService, GamerService>();
            services.AddTransient<GamerService>();
            services.AddTransient<GamersViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>(provider =>
                new MainWindow(provider.GetRequiredService<MainViewModel>()));
        }       
    }
}
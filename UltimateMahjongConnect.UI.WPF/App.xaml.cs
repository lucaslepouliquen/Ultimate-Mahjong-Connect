using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Database.Net.Models;
using UltimateMahjongConnect.Service;
using UltimateMahjongConnect.Service.Interface;
using UltimateMahjongConnect.Service.Services;
using UltimateMahjongConnect.UI.WPF.Interfaces;
using UltimateMahjongConnect.UI.WPF.Profiles;
using UltimateMahjongConnect.UI.WPF.Services;
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
            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GamerEntityGamerItemVIewModelProfile());
                mc.AddProfile(new GamerDTOGamerItemViewModelProfile());
            });
            
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddTransient<IGamerService, MockGamerService>();
            services.AddTransient<IMahjongTile, MahjongTile>();
            services.AddTransient<IMahjongBoard, MahjongBoard>();
            services.AddTransient<IMahjongBoardService, MahjongBoardService>();
            services.AddTransient<MahjongBoardViewModel>();

            services.AddTransient<Gamer>();
            services.AddTransient<GamerService>();
            services.AddTransient<GamersViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>(provider =>
                new MainWindow(provider.GetRequiredService<MainViewModel>()));
        }
    }
}
using AutoMapper;
using UltimateMahjongConnect.Service.Profiles;
using UltimateMahjongConnect.UI.WPF.Profiles;

namespace UltimateMahjongConnect.UI.WPF.Tests
{
    public class AutoMapperConfigForTests
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GamerEntityDTOProfile>();
                cfg.AddProfile<GamerDTOGamerItemViewModelProfile>();
                cfg.AddProfile<GamerEntityGamerItemVIewModelProfile>();
            });
            return config.CreateMapper();
        }
    }
}

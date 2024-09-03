using Microsoft.Extensions.DependencyInjection;
using UltimateMahjongConnect.Service.Profiles;

namespace UltimateMahjongConnect.Service
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GamerEntityDTOProfile));
        }
    }
}

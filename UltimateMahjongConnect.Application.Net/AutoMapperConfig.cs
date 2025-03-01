using Microsoft.Extensions.DependencyInjection;
using UltimateMahjongConnect.Application.Profiles;

namespace UltimateMahjongConnect.Application
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GamerEntityDTOProfile));
        }
    }
}

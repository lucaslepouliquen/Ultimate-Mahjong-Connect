using AutoMapper;
using UltimateMahjongConnect.Core.Net.Models;

namespace UltimateMahjongConnect.Service.Profiles
{
    public class GamerProfile : Profile
    {
        public GamerProfile()
        {
            {
                CreateMap<Database.Net.Models.Gamer, Gamer>()
                    .ForMember(dest => dest.Age, opt => opt.Ignore()) 
                    .ForMember(dest => dest.Score, opt => opt.Ignore()) 
                    .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => src.Pseudonyme))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            }
        }
    }
}

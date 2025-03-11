using AutoMapper;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Profiles
{
    public class GamerDTOProfile : Profile
    {
        public GamerDTOProfile()
        {
            {
                CreateMap<Gamer, GamerDTO>().ReverseMap()
                    .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
            }
        }
    }
}

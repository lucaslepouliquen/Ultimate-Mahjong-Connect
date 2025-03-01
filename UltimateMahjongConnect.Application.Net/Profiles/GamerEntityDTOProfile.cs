using AutoMapper;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Profiles
{
    public class GamerEntityDTOProfile : Profile
    {
        public GamerEntityDTOProfile()
        {
            {
                CreateMap<GamerEntity, GamerDTO>().ReverseMap()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => src.Pseudonyme))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                    .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
            }
        }
    }
}

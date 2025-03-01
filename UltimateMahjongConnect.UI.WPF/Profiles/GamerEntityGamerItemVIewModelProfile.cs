using AutoMapper;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF.Profiles
{
    public class GamerEntityGamerItemVIewModelProfile : Profile
    {
        public GamerEntityGamerItemVIewModelProfile()
        {
            CreateMap<GamerEntity, GamerItemViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => src.Pseudonyme))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score ?? 0))  
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => src.Pseudonyme))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score == 0 ? (int?)null : src.Score));
        }
    }
}

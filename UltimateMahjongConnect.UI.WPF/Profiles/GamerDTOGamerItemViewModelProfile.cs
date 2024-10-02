using AutoMapper;
using UltimateMahjongConnect.Service.DTO;
using UltimateMahjongConnect.UI.WPF.Model;
using UltimateMahjongConnect.UI.WPF.ViewModel;

namespace UltimateMahjongConnect.UI.WPF.Profiles
{
    public class GamerDTOGamerItemViewModelProfile : Profile
    {
        public GamerDTOGamerItemViewModelProfile()
        {
            CreateMap<GamerDTO, GamerItemViewModel>()
                .ConstructUsing(dto => new GamerItemViewModel(new GamerModel
                {
                    Id = dto.Id,
                    Pseudonyme = dto.Pseudonyme,
                    Password = dto.Password,
                    Score = dto.Score
                }))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => src.Pseudonyme))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
        }
    }
}

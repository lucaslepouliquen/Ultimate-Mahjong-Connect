using AutoMapper;
using System.Reflection;
using UltimateMahjongConnect.Application.DTO;
using UltimateMahjongConnect.Domain.Models;

namespace UltimateMahjongConnect.Application.Profiles
{
    public class GamerDTOProfile : Profile
    {
        public GamerDTOProfile()
        {
            CreateMap<Gamer, GamerDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID will be set separately
                .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => GetPrivateField(src, "_pseudonyme") ?? "Unknown"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => GetPrivateField(src, "_age") ?? 0))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetPrivateField(src, "_email") ?? "unknown@example.com"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => GetPrivateField(src, "_password") ?? "default"))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
        }

        // Helper method to get private field values using reflection
        private static object GetPrivateField(object obj, string fieldName)
        {
            return obj.GetType()
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(obj);
        }
    }
}

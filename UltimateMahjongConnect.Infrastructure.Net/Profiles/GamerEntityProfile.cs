using AutoMapper;
using UltimateMahjongConnect.Infrastructure.Persistence;
using UltimateMahjongConnect.Domain.Models;
using System.Reflection;

namespace UltimateMahjongConnect.Infrastructure.Profiles
{
    public class GamerEntityProfile : Profile
    {
        public GamerEntityProfile()
        {
            CreateMap<GamerEntity, Gamer>()
                .ConstructUsing((src, ctx) => 
                    new Gamer(
                        pseudonyme: src.Pseudonyme,
                        age: src.Age,
                        email: src.Email
                    ))
                .AfterMap((src, dest) => {
                    if (src.Score.HasValue && src.Score.Value > 0)
                    {
                        dest.AddScore(src.Score.Value);
                    }
                });

            CreateMap<Gamer, GamerEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID will be set separately
                .ForMember(dest => dest.Pseudonyme, opt => opt.MapFrom(src => GetPrivateField(src, "_pseudonyme") ?? "Unknown"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => GetPrivateField(src, "_age") ?? 0))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetPrivateField(src, "_email") ?? "unknown@example.com"))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => GetPrivateField(src, "_password") ?? "default"))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.BankDetails, opt => opt.MapFrom(src => ""));
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
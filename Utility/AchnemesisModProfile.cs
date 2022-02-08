using ArchnemesisRecipies.Models;
using AutoMapper;

namespace ArchnemesisRecipies.Utility
{
    public class AchnemesisModProfile : Profile
    {
        public AchnemesisModProfile()
        {
            CreateMap<ArchnemesisMod, ArchnemesisModViewModel>()
                .ForMember(x => x.Name, src => src.MapFrom(x => x.Name))
                .ForMember(x => x.Image, src => src.MapFrom(x => x.Image))
                .ForMember(x => x.Mod, src => src.MapFrom(x => x.Mod))
                .ForMember(x => x.Effect, src => src.MapFrom(x => x.Effect.Trim()))
                .ForMember(x => x.Regex, src => src.MapFrom(x => x.Regex))
                .ForMember(x => x.ComponentNames, src => src.MapFrom(x => x.ComponentNames))
                .ForMember(x => x.Components, src => src.Ignore());
        }
    }
}

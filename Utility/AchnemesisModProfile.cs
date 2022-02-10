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

            // export recipe maps
            CreateMap<RecipeViewModel, RecipeExportModel>()
                .ForMember(x => x.SelectedMods, src => src.MapFrom(x => x.SelectedMods))
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components));

            CreateMap<ArchnemesisModViewModel, string>()
                .ConvertUsing(x => x.Name);
            
            CreateMap<RecipeComponentViewModel, RecipeComponentExportModel>()
                .ForMember(x => x.Component, src => src.MapFrom(x => x.Component.Name))
                .ForMember(x => x.Count, src => src.MapFrom(x => x.Count));

            // import recipe maps
            CreateMap<RecipeExportModel, RecipeViewModel>()
                .ForMember(x => x.SelectedMods, src => src.MapFrom(x => x.SelectedMods))
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components));

            CreateMap<string, ArchnemesisModViewModel>()
                .ConvertUsing(x => new ArchnemesisModViewModel { Name = x });

            CreateMap<RecipeComponentExportModel, RecipeComponentViewModel>()
                .ConvertUsing(x => new RecipeComponentViewModel
                {
                    Component = new ArchnemesisModViewModel { Name = x.Component },
                    Count = x.Count
                });                
        }
    }
}

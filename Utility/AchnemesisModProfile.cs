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
                .ForMember(x => x.Components, src => src.Ignore())
                .ForMember(x => x.Maps, src => src.MapFrom(x => x.Maps))
                .ForMember(x => x.ExpressionEvaluator, src => src.MapFrom(x => RewardExpressionEvaluators.GetEvaluator(x.Effect)))
                ;

            CreateMap<ArchnemesisModViewModel, RecipeComponentViewModel>()
                .ForMember(x => x.Name, src => src.MapFrom(x => x.Name))
                .ForMember(x => x.Image, src => src.MapFrom(x => x.Image))
                .ForMember(x => x.Effect, src => src.MapFrom(x => x.Effect))
                .ForMember(x => x.Regex, src => src.MapFrom(x => x.Regex))
                .ForMember(x => x.ModTier, src => src.MapFrom(x => x.ModTier))
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components))
                .ForMember(x => x.Rewards, src => src.Ignore())
                .ForMember(x => x.IsCompleted, src => src.Ignore())
                .ForMember(x => x.ExpressionEvaluator, src => src.MapFrom(x => x.ExpressionEvaluator));

            CreateMap<RecipeViewModel, RecipeExportModel>()
                .ForMember(x => x.SelectedMods, src => src.MapFrom(x => x.SelectedMods));

            CreateMap<RecipeExportModel, RecipeViewModel>()
                .ForMember(x => x.SelectedMods, src => src.MapFrom(x => x.SelectedMods));

            CreateMap<RecipeComponentExportModel, RecipeComponentViewModel>()
                .ForMember(x => x.Name, src => src.MapFrom(x => x.Name))
                .ForMember(x => x.IsCompleted, src => src.MapFrom(x => x.IsCompleted))
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components));

            CreateMap<RecipeComponentViewModel, RecipeComponentExportModel>()
                .ForMember(x => x.Name, src => src.MapFrom(x => x.Name))
                .ForMember(x => x.IsCompleted, src => src.MapFrom(x => x.IsCompleted));
        }
    }
}

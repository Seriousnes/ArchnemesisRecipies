using ArchnemesisRecipies.Models;
using AutoMapper;

namespace ArchnemesisRecipies.Utility
{
    public class AchnemesisModProfile : Profile
    {
        public AchnemesisModProfile()
        {
            CreateMap<ArchnemesisMod, ArchnemesisModViewModel>()
                .ForMember(x => x.Components, src => src.Ignore())                
                .ForMember(x => x.ExpressionEvaluator, src => src.MapFrom(x => RewardExpressionEvaluators.GetEvaluator(x.Effect)))
                ;

            CreateMap<ArchnemesisModViewModel, RecipeComponentViewModel>()
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components))
                .ForMember(x => x.Rewards, src => src.Ignore())
                .ForMember(x => x.IsCompleted, src => src.Ignore())
                ;

            CreateMap<RecipeViewModel, RecipeExportModel>()
                .ForMember(x => x.SelectedMods, src => src.MapFrom(x => x.SelectedMods))
                .ReverseMap()
                ;

            CreateMap<RecipeComponentExportModel, RecipeComponentViewModel>()
                .ForMember(x => x.Components, src => src.MapFrom(x => x.Components));

            CreateMap<RecipeComponentViewModel, RecipeComponentExportModel>();                
        }
    }
}

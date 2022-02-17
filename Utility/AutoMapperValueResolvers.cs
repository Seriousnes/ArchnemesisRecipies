using ArchnemesisRecipies.Models;
using AutoMapper;

namespace ArchnemesisRecipies.Utility
{
    public class RecipeComponentCollectionResolver : IValueResolver<ArchnemesisModViewModel, RecipeComponentViewModel, List<RecipeComponentViewModel>>
    {
        private static readonly IMapper _mapper = WrappedMapper.Mapper;
        public List<RecipeComponentViewModel> Resolve(ArchnemesisModViewModel source, RecipeComponentViewModel destination, List<RecipeComponentViewModel> destMember, ResolutionContext context)
        {
            var result = new List<RecipeComponentViewModel>();
            foreach (var item in source.Components)
            {
                result.Add(_mapper.Map<RecipeComponentViewModel>(item));
            }
            return result;
        }
    }

    public class MapTierResolver : ITypeConverter<string, Tier>
    {
        public Tier Convert(string source, Tier destination, ResolutionContext context) => Extensions.FromString<Tier>(source);
    }
}

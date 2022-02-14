using AutoMapper;
using System.Text;
using System.Text.Json;

namespace ArchnemesisRecipies.Models
{
    public class RecipeViewModel
    {
        private readonly IMapper _mapper;

        public RecipeViewModel()
        {
        }

        public RecipeViewModel(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<RecipeComponentViewModel> SelectedMods { get; set; } = new();        
        public string Export(IMapper mapper)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mapper.Map<RecipeViewModel, RecipeExportModel>(this))));
        }
        public static RecipeViewModel Import(string importString, IMapper mapper)
        {
            try
            {
                return mapper.Map<RecipeExportModel, RecipeViewModel>(JsonSerializer.Deserialize<RecipeExportModel>(Convert.FromBase64String(importString)));
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public void AddMod(RecipeComponentViewModel mod)
        {
            //var recipeComponent = _mapper.Map<ArchnemesisModViewModel, RecipeComponentViewModel>(mod);
            SelectedMods = SelectedMods.Append(mod).ToList();
        }

        public void RemoveMod(RecipeComponentViewModel mod)
        {
            //SelectedMods = SelectedMods.Where(x => x.Name != mod.Name).ToList();
            SelectedMods.Remove(mod);
        }

        //public bool ContainsMod(ArchnemesisModViewModel mod) => SelectedMods.Any(x => x.Name == mod.Name);
        
        public IEnumerable<(IEnumerable<string> images, string modifier)> CalculateRewards()
        {
            var activeEffects = new List<(IExpressionEvaluator evaluator, string value)>();
            List<string> unmodifiedRewards = new();            

            foreach (var mod in SelectedMods)
            {                
                if (mod.ExpressionEvaluator is var exprEvaluator)
                {
                    // only use the first type conversion evaluator
                    if (exprEvaluator is AllRewardsAreThisExpressionEvaluator)
                    {
                        if (!activeEffects.OfType<AllRewardsAreThisExpressionEvaluator>().Any())
                            activeEffects.Add((mod.ExpressionEvaluator, mod.Effect));
                    }
                    else
                    {
                        activeEffects.Add((mod.ExpressionEvaluator, mod.Effect));
                    }
                }

                // get reward types
                unmodifiedRewards.AddRange(mod.Rewards);
                var rewards = string.Join(" ", unmodifiedRewards);
                var modifier = "";
                foreach (var (evaluator, value) in activeEffects)
                {
                    rewards = evaluator is IRewardsEvaluator rewardEvaluator ? rewardEvaluator.GetExpression(value)(rewards) : rewards;
                    modifier = evaluator is IRarityEvaluator rarityEvaluator ? rarityEvaluator.GetExpression(value)(modifier) : modifier;
                }

                yield return (rewards.GetImageUrls(false, "small-image"), modifier);
            }
        }
    }

    public class RecipeComponentViewModel
    {
        public string Name { get; set; }
        public List<RecipeComponentViewModel> Components { get; set; }
        public string Type { get; set; }
        public string Effect { get; set; }
        public string Regex { get; set; }
        public IEnumerable<string> Rewards => Type.Split(" ");
        public IExpressionEvaluator ExpressionEvaluator { get; set; }
        public bool IsCompleted { get; set; }
        public string Image { get; set; }
    }


    public class RecipeExportModel
    {
        public List<RecipeComponentExportModel> SelectedMods { get; set; } = new();
    }

    public class RecipeComponentExportModel
    {
        public string Component { get; set; }
        public bool IsCompleted { get; set; }
    }
}

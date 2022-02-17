using ArchnemesisRecipies.Utility;
using AutoMapper;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArchnemesisRecipies.Models
{
    public class RecipeViewModel
    {
        private readonly IMapper _mapper = WrappedMapper.Mapper;

        public RecipeViewModel()
        {
        }

        public List<RecipeComponentViewModel> SelectedMods { get; set; } = new();        
        
        public string Export()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_mapper.Map<RecipeExportModel>(this))));
        }

        public void Import(string importString)
        {
            try
            {
                _mapper.Map(JsonSerializer.Deserialize<RecipeExportModel>(Convert.FromBase64String(importString)), this);
            }
            catch (Exception)
            {
            }
        }
        
        public void AddMod(RecipeComponentViewModel mod)
        {
            SelectedMods = SelectedMods.Append(mod).ToList();
        }

        public void RemoveMod(RecipeComponentViewModel mod)
        {
            SelectedMods.Remove(mod);
        }
      
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
                foreach (var (evaluator, value) in activeEffects.OrderByDescending(x => x.evaluator?.Priority))
                {
                    rewards = evaluator is IRewardsEvaluator rewardEvaluator ? rewardEvaluator.GetExpression(value)(rewards) : rewards;
                    modifier = evaluator is IRarityEvaluator rarityEvaluator ? rarityEvaluator.GetExpression(value)(modifier) : modifier;
                }

                yield return (rewards.GetImageUrls(false, "small-image"), modifier);
            }
        }
    }

    public class RecipeComponentViewModel : Entity
    {
        public List<RecipeComponentViewModel> Components { get; set; }
        [JsonIgnore]
        public int ModTier { get; set; }
        public string Type { get; set; }
        public string Effect { get; set; }
        public string Regex { get; set; }
        [JsonIgnore]
        public string Mod { get; set; }
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
        public string Name { get; set; }
        public List<RecipeComponentExportModel> Components { get; set; }
        public bool IsCompleted { get; set; }
    }
}

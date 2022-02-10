using AutoMapper;
using System.Text;
using System.Text.Json;

namespace ArchnemesisRecipies.Models
{
    public class RecipeViewModel
    {
        public List<ArchnemesisModViewModel> SelectedMods { get; set; } = new();
        public List<RecipeComponentViewModel> Components { get; set; } = new();

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

        public static RecipeViewModel GetRecipeViewModel(IEnumerable<ArchnemesisModViewModel> items)
        {
            return new RecipeViewModel
            {
                SelectedMods = items.ToList(),
                Components = items.SelectMany(x => GetComponentCount(x)).GroupBy(x => x).Select(x => new RecipeComponentViewModel { Component = x.Key, Count = x.Count() }).OrderBy(x => x.Count).ToList()
            };
        }

        private static List<ArchnemesisModViewModel> GetComponentCount(ArchnemesisModViewModel mod)
        {
            var result = mod.Components.Where(x => x.ModTier == 1).ToList();
            result.AddRange(mod.Components.Where(x => x.ModTier > 1).SelectMany(x => GetComponentCount(x)));
            return result;
        }
    }

    public class RecipeComponentViewModel
    {
        public ArchnemesisModViewModel Component { get; set; }
        public int Count { get; set; }
    }


    public class RecipeExportModel
    {
        public List<string> SelectedMods { get; set; } = new();
        public List<RecipeComponentExportModel> Components { get; set; } = new();
    }

    public class RecipeComponentExportModel
    {
        public string Component { get; set; }
        public int Count { get; set; }
    }
}

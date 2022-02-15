using ArchnemesisRecipies.Utility;
using AutoMapper;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ArchnemesisRecipies.Models
{
    public static class Extensions
    {
        private static IMapper _mapper = WrappedMapper.Mapper;

        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();
            var memberInfo = type.GetMember(element.ToString());

            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return element.ToString();
        }

        public static IEnumerable<T> FlattenRecursiveList<T>(this IEnumerable<T> list, Func<T, IEnumerable<T>> select, Func<T, bool> when)
        {
            var result = new List<T>();
            result.AddRange(list);
            if (list.Where(when).Count() > 0)
            {
                result.AddRange(list.FlattenRecursiveList(select, when));
            }
            return result;
        }

        public static void Map(this RecipeViewModel model, IEnumerable<ArchnemesisModViewModel> mods)
        {
            foreach (var component in model.SelectedMods)
            {
                component.Map(mods);
            }    
        }

        private static void Map(this RecipeComponentViewModel model, IEnumerable<ArchnemesisModViewModel> mods)
        {
            // remove components before mapping
            var components = model.Components;
            model.Components = null;
            
            foreach (var component in components)
            {
                component.Map(mods);
            }

            _mapper.Map(mods.SingleOrDefault(x => x.Name == model.Name), model);
            model.Components = components;
        }

        public static Dictionary<RecipeComponentViewModel, int> GetComponents(this RecipeViewModel model)
        {
            return model.SelectedMods
                .SelectMany(x => x.GetComponents())
                .GroupBy(x => x, new RecipeComponentViewModelEqualityComparer())
                .ToDictionary(x => x.Key, x => x.Count());
        }

        public static IEnumerable<RecipeComponentViewModel> GetComponents(this RecipeComponentViewModel model)
        {
            return model.ModTier switch
            {
                1 => new List<RecipeComponentViewModel> { model },
                _ => model.Components.Where(x => x.ModTier == 1).Concat(model.Components.Where(x => x.ModTier > 1).SelectMany(x => x.GetComponents()))
            };
        }

        public static Dictionary<RecipeComponentViewModel, int> GetComponentCount(this IEnumerable<RecipeComponentViewModel> components)
        {
            return components
                .GroupBy(x => x, new RecipeComponentViewModelEqualityComparer())
                .ToDictionary(x => x.Key, x => x.Count());
        }

        private class RecipeComponentViewModelEqualityComparer : IEqualityComparer<RecipeComponentViewModel>
        {
            public bool Equals(RecipeComponentViewModel? x, RecipeComponentViewModel? y) => x?.Name == y?.Name;

            public int GetHashCode([DisallowNull] RecipeComponentViewModel obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static IEnumerable<string> GetImageUrls(this string rewards, bool group = true, string imgStyle = "")
        {
            return GetImageUrls(rewards.Split(" "), group, imgStyle);
        }

        public static IEnumerable<string> GetImageUrls(this IEnumerable<string> rewards, bool group = true, string imgStyle = "")
        {
            foreach (var rewardGroup in rewards.GroupBy(x => x))
            {
                if (_rewards.TryGetValue(rewardGroup.Key, out var url))
                {
                    if (rewardGroup.Count() > 1)
                    {
                        if (group)
                        {
                            var result = new List<string>();
                            for (var i = 0; i < rewardGroup.Count(); i++)
                            {
                                var @class = "group";
                                if (i + 1 == rewardGroup.Count())
                                {
                                    @class += " group-end";
                                }
                                if (i == 0)
                                {
                                    @class += " group-start";
                                }

                                result.Add($"<img class=\"{imgStyle} {@class}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{rewardGroup.Key}\" />");
                            }

                            foreach (var r in result)
                            {
                                yield return r;
                            }
                        }
                        else
                        {
                            yield return $"<span class=\"effects\">{(rewardGroup.Count() > 1 ? $"{rewardGroup.Count()} x" : "")}<img class=\"{imgStyle}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{rewardGroup.Key}\" /></span>";
                        }
                    }
                    else
                    {
                        yield return $"<img class=\"{imgStyle}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{rewardGroup.Key}\" />";
                    }


                }
                else
                {
                    yield return "";
                }
            }
        }

        private static Dictionary<string, string> _rewards = new()
        {
            { "Generic", "images/rewards/HeistRewardGeneric.png" },
            { "Gems", "images/rewards/HeistRewardGems.png" },
            { "Armour", "images/rewards/HeistRewardArmour.png" },
            { "Abyss", "images/rewards/HeistRewardAbyss.png" },
            { "Blight", "images/rewards/HeistRewardBlight.png" },
            { "Breach", "images/rewards/HeistRewardBreach.png" },
            { "Currency", "images/rewards/HeistRewardCurrency.png" },
            { "Delirium", "images/rewards/HeistRewardDelirium.png" },
            { "DivinationCards", "images/rewards/HeistRewardDivination.png" },
            { "Essences", "images/rewards/HeistRewardEssences.png" },
            { "Fossils", "images/rewards/HeistRewardFossils.png" },
            { "Fragments", "images/rewards/HeistRewardFragments.png" },
            { "Harbinger", "images/rewards/HeistRewardHarbinger.png" },
            { "Labyrinth", "images/rewards/HeistRewardLabyrinth.png" },
            { "Legion", "images/rewards/HeistRewardLegion.png" },
            { "Maps", "images/rewards/HeistRewardMaps.png" },
            { "Metamorphosis", "images/rewards/HeistRewardMetamorph.png" },
            { "Scarabs", "images/rewards/HeistRewardScarabs.png" },
            { "Trinkets", "images/rewards/HeistRewardTrinkets.png" },
            { "Uniques", "images/rewards/HeistRewardUniques.png" },
            { "Weapon", "images/rewards/HeistRewardWeapon.png" },
            { "Expedition", "images/rewards/RewardIconExpedition.png" },
            { "Heist", "images/rewards/RewardIconHeist.png" },
            { "Ritual", "images/rewards/RewardIconRitual.png" }
        };
    }
}

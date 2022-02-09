using System.Diagnostics;

namespace ArchnemesisRecipies.Models
{
    [DebuggerDisplay("{Name} (T{ModTier})")]
    public class ArchnemesisModViewModel : ArchnemesisMod
    {
        public List<ArchnemesisModViewModel> Components { get; set; }
        public List<ArchnemesisModViewModel> ComponentOf { get; set; }
        public int ModTier => (Components?.Any() ?? false ? Components.Max(x => x.ModTier) : 0) + 1;
        public string HighlightStyle { get; set; }
        public bool Fade { get; set; }
        public bool MouseOver { get; set; }
        public bool Selected { get; set; }

        public string GetStyle()
        {
            return string.Join(" ", HighlightStyle, Fade ? "faded" : "");
        }

        public IEnumerable<string> GetImageUrls(bool group = true, string imgStyle = "")
        {
            if (group)
            {
                foreach (var rewardGroup in Type.Split(" ").GroupBy(x => x))
                {
                    if (_rewards.TryGetValue(rewardGroup.Key, out var url))
                    {
                        if (rewardGroup.Count() > 1)
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
                            yield return $"<img class=\"{imgStyle}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{rewardGroup.Key}\" />";
                        }    
                        
                        //yield return $"<span class=\"effects\">{(reward.Count() > 1 ? $"{reward.Count()} x" : "")}<img class=\"{imgStyle}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{reward.Key}\" /></span>";
                    }
                    else
                    {
                        yield return "";
                    }                    
                }
            }
            else
            {
                foreach (var reward in Type.Split(" "))
                {
                    if (_rewards.TryGetValue(reward, out var url))
                    {
                        yield return $"<img class=\"{imgStyle}\" src=\"{url}\" data-bs-toggle=\"tooltip\" data-bs-placement=\"top\" title=\"{reward}\" />";
                    }
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

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ArchnemesisRecipies.Models
{    
    public enum RecipeTab
    {
        [Description("Build Tree")]
        BuildTree,
        [Description("T1 Mod Count")]
        T1ModCount,
        [Description("Archnemesis mods")]
        ArchnemesisMods,
        Details
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Tier
    {
        Any,
        Low,
        Mid,
        High
    }

    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Influence
    {
        None,
        [Description("Delirium")]
        Delirious,
        Elder,
        Blight,
        Shaper,
        UberBlight,
    }
}

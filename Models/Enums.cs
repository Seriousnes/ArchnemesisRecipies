using System.ComponentModel;

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
}

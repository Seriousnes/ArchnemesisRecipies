using System.Diagnostics;

namespace ArchnemesisRecipies.Models
{
    [DebuggerDisplay("{Name} (T{ModTier})")]
    public class ArchnemesisModViewModel : ArchnemesisMod
    {
        public List<ArchnemesisModViewModel> Components { get; set; }
        public List<ArchnemesisModViewModel> ComponentOf { get; set; }
        public int ModTier => (Components?.Any() ?? false ? Components.Max(x => x.ModTier) : 0) + 1;
        public Stack<string> Class { get; set; } = new();
        public string Style { get; set; } 
        public bool MouseOver { get; set; }
        public bool Selected { get; set; }
    }
}

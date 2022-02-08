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
    }
}

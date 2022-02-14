using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ArchnemesisRecipies.Models
{
    [DebuggerDisplay("{Name} (T{ModTier})")]
    public class ArchnemesisModViewModel : ArchnemesisMod
    {
        [JsonIgnore]
        public virtual List<ArchnemesisModViewModel> Components { get; set; }
        [JsonIgnore]
        public List<ArchnemesisModViewModel> ComponentOf { get; set; }
        [JsonIgnore]
        public int ModTier => (Components?.Any() ?? false ? Components.Max(x => x.ModTier) : 0) + 1;
        [JsonIgnore]
        public string HighlightStyle { get; set; }
        [JsonIgnore]
        public bool Fade { get; set; }
        [JsonIgnore]
        public bool MouseOver { get; set; }
        [JsonIgnore]
        public bool Selected { get; set; }
        [JsonIgnore]
        public IEnumerable<string> Rewards => Type.Split(" ");
        [JsonIgnore]
        public IExpressionEvaluator ExpressionEvaluator { get; set; }

        public string GetStyle(string additionalHighlights = "")
        {
            var highlightStyle = HighlightStyle;
            if (!string.IsNullOrEmpty(highlightStyle) && !string.IsNullOrEmpty(additionalHighlights))
            {
                highlightStyle += $" {additionalHighlights}";
            }
            return string.Join(" ", highlightStyle, Fade ? "faded" : "");
        }
    }
}

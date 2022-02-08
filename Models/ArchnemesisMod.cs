using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ArchnemesisRecipies.Models
{
    [DebuggerDisplay("{Name}")]
    public class ArchnemesisMod
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Mod { get; set; }
        public string Type { get; set; }
        [JsonPropertyName("Components")]
        public List<string> ComponentNames { get; set; } = new();
        public string Effect { get; set; }
    }
}

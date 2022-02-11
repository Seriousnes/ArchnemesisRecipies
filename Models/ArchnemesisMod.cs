using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ArchnemesisRecipies.Models
{
    [DebuggerDisplay("{Name}")]
    public class ArchnemesisMod : Entity
    {
        public string Image { get; set; }
        public string Mod { get; set; }
        public string Type { get; set; }
        [JsonPropertyName("Components")]
        public List<string> ComponentNames { get; set; } = new();
        public string Effect { get; set; }
        public string Regex { get; set; }
        public List<Map> Maps { get; set; } = new();
    }
}

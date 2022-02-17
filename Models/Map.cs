namespace ArchnemesisRecipies.Models
{
    public class Map : Entity
    {
        public int MinimumTier { get; set; }
        public Influence Influence { get; set; }
        public string GetBase() => string.Format("images/maps/base/{0}.png", this.Influence switch
        {
            Influence.Delirious => "delirious",
            Influence.Elder => "elder",
            Influence.Blight => "infected",
            Influence.Shaper => "shaper",
            Influence.UberBlight => "uberblight",
            _ => "baseicon"
        });
        public string GetGlyph() => $"images/maps/glyphs/{Name.ToLower().Replace(" ", "")}.png";

        public string GetTierStyle() => this.MinimumTier switch
        {
            >= 11 => "high-tier",
            >= 6 => "mid-tier",
            _ => ""
        };
    }
}

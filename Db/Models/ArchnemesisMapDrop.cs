namespace ArchnemesisRecipies.Db.Models
{
    public class ArchnemesisMapDrop : Entity<int>
    {
        public ArchnemesisMod ArchnemesisMod { get; set; }
        public Map Map { get; set; }        
    }
}

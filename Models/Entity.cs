namespace ArchnemesisRecipies.Models
{
    public abstract class Entity : IEntity
    {
        public string Name { get; set; }
        public string GetElementId()
        {
            return Name.ToLower().Replace(' ', '-');
        }
    }

    public interface IEntity
    {
        string Name { get; }
        string GetElementId();
    }
}

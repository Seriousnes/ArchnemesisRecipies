namespace ArchnemesisRecipies.Db.Models
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }
    }
}

namespace Opw.PineBlog.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}

namespace NHibernate.PropertyRefBug.Entities;

public class Order
{
    public virtual long Id { get; set; }

    public virtual string UniqueId { get; set; } = Guid.NewGuid().ToString();

    public virtual DateTime CreatedDate { get; set; }
}
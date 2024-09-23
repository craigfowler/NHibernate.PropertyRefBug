namespace NHibernate.PropertyRefBug.Entities;

public class LineItem
{
    public virtual long Id { get; set; }

    public virtual Order Order { get; set; }

    public virtual string ItemName { get; set; }
    
    public virtual decimal Amount { get; set; }
}
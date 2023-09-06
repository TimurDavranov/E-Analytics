namespace Domain.Primitives;

public abstract class AggregateRootSimple : BaseEntity<Guid>
{
    public Guid _id;
    
    public Guid Id
    {
        get { return _id; }
    }

    public AggregateRootSimple()
    {
        _id = Guid.NewGuid();
    }

}
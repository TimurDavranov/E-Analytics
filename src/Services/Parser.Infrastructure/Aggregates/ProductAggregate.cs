using EA.Domain.Primitives;

namespace Parser.Infrastructure.Aggregates;

public class ProductAggregate : AggregateRootSimple
{
    public ProductAggregate(Guid id, string name, Guid categoryId)
    {
        Name = name;
    }

    public string Name { get; private set; }
    
    


}
using EA.Domain.Primitives;
using EAnalytics.Common.Aggregates;

namespace Parser.Infrastructure.Aggregates;

public class CategoryModel : AggregateRootSimple
{
    public CategoryModel(Guid id, string name, Guid parentId)
    {
        Name = name;
    }

    public string Name { get; private set; }
    
    


}
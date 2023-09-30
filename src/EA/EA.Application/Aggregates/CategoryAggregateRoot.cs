using EA.Domain.Events;
using EA.Domain.Primitives;
using EAnalytics.Common.Aggregates;
using EAnalytics.Common.Dtos;

namespace EA.Application.Aggregates;

public class CategoryAggregateRoot : AggregateRootSimple
{
    private bool _active;
    public bool Active { get => _active; set => _active = value; }

    public CategoryAggregateRoot()
    {
    }

    public CategoryAggregateRoot(Guid Id, List<TranslationDto> translations)
    {
        RaiseEvent(new AddCategoryEvent()
        {
            Id = Id,
            Translations = translations
        });
    }

    public void Apply(AddCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }

    public void EditCategory(long categoryId, List<TranslationDto> translations)
    {
        RaiseEvent(new EditCategoryEvent()
        {
            Id = Id,
            CategoryId = categoryId,
            Translations = translations
        });
    }

    public void Apply(EditCategoryEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }

    public void AddProduct(string name, string serviceName, decimal price)
    {
        RaiseEvent(new AddProductEvent()
        {
            Id = _id,
            ProductId = Guid.NewGuid(),
            ProductSystemId = Guid.NewGuid(),
            Name = name,
            Price = price,
            ServiceName = serviceName,
            
        });
    }

    public void Apply(AddProductEvent @event)
    {
        _id = @event.Id;
        _active = true;
    }
}
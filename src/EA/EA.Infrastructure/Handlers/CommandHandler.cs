using EA.Application.Aggregates;
using EA.Domain.Events;
using EA.Infrastructure.Commands.Categories;
using EA.Infrastructure.Commands.Products;

namespace EA.Infrastructure.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddCategoryCommand command);
        Task HandleAsync(EditCategoryCommand command);
        Task HandleAsync(AddProductCommand command);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<CategoryAggregateRoot> _eventSourcingHandler;
        public CommandHandler(IEventSourcingHandler<CategoryAggregateRoot> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public Task HandleAsync(AddCategoryCommand command)
        {
            var aggregate = new CategoryAggregateRoot(command.Id, command.Translations);

            
            return _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCategoryCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            if (aggregate is null)
                throw new ArgumentNullException($"Aggregate with this ID: {command.Id} not found!");

            aggregate.EditCategory(command.CategoryId, command.Translations);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddProductCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            
            if (aggregate is null)
                throw new ArgumentNullException($"Aggregate with this ID: {command.Id} not found!");
            
            aggregate.AddProduct(command.Name,command.SystemName, command.Price, command.Url);

            await _eventSourcingHandler.SaveAsync(aggregate);

        }

        
    }
}
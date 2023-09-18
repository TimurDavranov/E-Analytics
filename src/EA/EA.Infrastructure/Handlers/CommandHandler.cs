using EA.Application.Aggregates;
using EA.Infrastructure.Commands.Categories;

namespace EA.Infrastructure.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddCategoryCommand command);
        Task HandleAsync(EditCategoryCommand command);
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
    }
}
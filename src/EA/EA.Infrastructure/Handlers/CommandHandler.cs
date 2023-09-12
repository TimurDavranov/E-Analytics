using EA.Application.Aggregates;
using EA.Infrastructure.Commands.Categories;

namespace EA.Infrastructure.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddCategoryCommand command);
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
    }
}
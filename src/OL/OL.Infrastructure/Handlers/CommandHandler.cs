using EAnalytics.Common.Handlers;
using OL.Application.Aggregates.Category;
using OL.Domain;
using OL.Infrastructure.Commands.Categories;

namespace OL.Infrastructure.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddOLCategoryCommand command);
        Task HandleAsync(EnableOLCategoryCommand command);
    }

    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<OLCategoryAggregateRoot> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<OLCategoryAggregateRoot> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public Task HandleAsync(AddOLCategoryCommand command)
        {
            var aggregate = new OLCategoryAggregateRoot(command.Id, command.SystemId, command.ParentId, command.Translations.ToList());

            return _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EnableOLCategoryCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            if (aggregate is null)
                throw new ArgumentNullException($"Aggregate with this ID: {command.Id} not found!");

            aggregate.EnableCategory(command.Id, command.Enabled);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
using EAnalytics.Common.Exceptions;
using EAnalytics.Common.Handlers;
using OL.Application.Aggregates.Category;
using OL.Application.Aggregates.Product;
using OL.Domain;
using OL.Infrastructure.Commands.Categories;
using OL.Infrastructure.Commands.Product;

namespace OL.Infrastructure.Handlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddOlCategoryCommand command);
        Task HandleAsync(UpdateOlCategoryCommand command);
        Task HandleAsync(EnableOLCategoryCommand command);
        
        Task HandleAsync(AddOlProductCommand command);
        Task HandleAsync(UpdateOlProductCommand command);
    }

    public class CommandHandler(IEventSourcingHandler<OLCategoryAggregateRoot> categoryEventSourcingHandler, IEventSourcingHandler<OLProductAggregateRoot> productEventSourcingHandler)
        : ICommandHandler
    {
        public Task HandleAsync(AddOlCategoryCommand command)
        {
            var aggregate = new OLCategoryAggregateRoot(command.Id, command.SystemId, command.ParentId, command.Translations.ToList());

            return categoryEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(UpdateOlCategoryCommand command)
        {
            var aggregate = await categoryEventSourcingHandler.GetByIdAsync(command.Id);

            if (aggregate is null)
                throw new AggregateNotFoundException($"Aggregate with this ID: {command.Id} not found!");

            aggregate.UpdateCategory(command.Id, command.SystemId, command.ParentId, command.Translations.AsReadOnly());

            await categoryEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EnableOLCategoryCommand command)
        {
            var aggregate = await categoryEventSourcingHandler.GetByIdAsync(command.Id);

            if (aggregate is null)
                throw new AggregateNotFoundException($"Aggregate with this ID: {command.Id} not found!");

            aggregate.EnableCategory(command.Id, command.Enabled);

            await categoryEventSourcingHandler.SaveAsync(aggregate);
        }

        public Task HandleAsync(AddOlProductCommand command)
        {
            var aggregate = new OLProductAggregateRoot(command.Id, command.SystemId, command.Translations.AsReadOnly());
            return categoryEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(UpdateOlProductCommand command)
        {
            var aggregate = await productEventSourcingHandler.GetByIdAsync(command.Id);
            if (aggregate is null)
                throw new AggregateNotFoundException($"Aggregate with this ID: {command.Id} not found!");
            
            aggregate.UpdateCategory(command.Id, command.Translations.AsReadOnly());
        }
    }
}
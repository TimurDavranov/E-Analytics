using EAnalytics.Common.Abstractions.Repositories;
using OL.Domain;
using OL.Domain.Entities;
using OL.Domain.Primitives.Entities;
using OL.Infrastructure;

namespace OL.Parser.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(AddOLCategoryEvent @event);
        Task On(EnableOLCategoryEvent @event);
    }

    public class EventHandler : IEventHandler
    {
        private readonly IRepository<OLCategory, OLDbContext> _categoryRepository;

        public EventHandler(IRepository<OLCategory, OLDbContext> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task On(AddOLCategoryEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            if (@event.ParentId is 0)
                throw new InvalidDataException("Incorrect Parrent Id is sended!");

            return _categoryRepository.CreateAsync(new OLCategory
            {
                Id = @event.Id,
                Translations = @event.Translations.Select(s => new OLTranslation
                {
                    Title = s.Title,
                    Description = s.Description,
                    LanguageCode = s.LanguageCode.Code
                }).ToList(),
                Enabled = true,
                SystemId = @event.SystemId,
                ParrentId = @event.ParentId
            });
        }

        public Task On(EnableOLCategoryEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}

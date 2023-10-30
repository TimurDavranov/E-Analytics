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
                Translations = new List<OLTranslation>
                {
                    new OLTranslation
                    {
                        Title = @event.NameRu,
                        Description = string.Empty,
                        LanguageCode = "ru"
                    },
                    new OLTranslation
                    {
                        Title = @event.NameUz,
                        Description = string.Empty,
                        LanguageCode = "uz-Cyrl"
                    },
                    new OLTranslation
                    {
                        Title = @event.NameOz,
                        Description = string.Empty,
                        LanguageCode = "uz"
                    },
                    new OLTranslation
                    {
                        Title = @event.NameEn,
                        Description = string.Empty,
                        LanguageCode = "en"
                    }
                },
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

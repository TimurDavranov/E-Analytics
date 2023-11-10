using EAnalytics.Common;
using EAnalytics.Common.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
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

            var category = _categoryRepository.Get(s => s.SystemId == s.SystemId, i => i.Include(s => s.Translations));

            if (category is not null)
            {
                category.ParrentId = @event.ParentId;
                category.SystemId = @event.SystemId;
                foreach (var lang in SupportedLanguageCodes.Codes)
                {
                    if (category.Translations.Any(s => s.LanguageCode == lang) && @event.Translations.Any(s => s.LanguageCode.Equal(lang)))
                    {
                        category.Translations.FirstOrDefault(s => s.LanguageCode == lang)!.Title = @event.Translations.FirstOrDefault(s => s.LanguageCode.Code == lang)!.Title;
                    }
                    else if (category.Translations.Any(s => s.LanguageCode == lang) && !@event.Translations.Any(s => s.LanguageCode.Equal(lang)))
                    {
                        category.Translations.FirstOrDefault(s => s.LanguageCode == lang)!.IsDeleted = true;
                    }
                    else if (!category.Translations.Any(s => s.LanguageCode == lang) && @event.Translations.Any(s => s.LanguageCode.Equal(lang)))
                    {
                        category.Translations.Add(new OLTranslation
                        {
                            LanguageCode = @event.Translations.FirstOrDefault(s => s.LanguageCode.Equal(lang))!.LanguageCode.Code,
                            Title = @event.Translations.FirstOrDefault(s => s.LanguageCode.Equal(lang))!.Title,
                            Description = string.Empty
                        });
                    }
                }
                return _categoryRepository.UpdateAsync(category);
            }

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

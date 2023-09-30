using EA.Domain.Abstraction.Repositories;
using EA.Domain.Entities;
using EA.Domain.Events;
using EA.Domain.Primitives.Entities;
using Microsoft.EntityFrameworkCore;

namespace Parser.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(AddCategoryEvent @event);
        Task On(AddProductEvent @event);
    }

    public class EventHandler : IEventHandler
    {
        private readonly IRepository<Category> _categoryRepository;

        public EventHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task On(AddCategoryEvent @event)
        {
            var category = new Category()
            {
                Translations = @event.Translations.Select(s => new EATranslation
                {
                    LanguageCode = s.LanguageCode,
                    Description = s.Description,
                    Title = s.Title
                }).ToList()
            };

            return _categoryRepository.CreateAsync(category);
        }

        public async Task On(EditCategoryEvent @event)
        {
            if (@event.CategoryId is 0)
                throw new InvalidDataException("Incorrect category Id is sended!");

            var category = await _categoryRepository.GetAsync(s => s.Id == @event.CategoryId, i => i.Include(s => s.Translations));

            if (category is null)
                throw new ArgumentNullException($"Category with this ID: {@event.CategoryId} not found!");

            @event.Translations.ForEach(c =>
            {
                if (category.Translations.Any(s => s.LanguageCode == c.LanguageCode))
                {
                    category.Translations.FirstOrDefault(s => s.LanguageCode == c.LanguageCode).Title = c.Title;
                    category.Translations.FirstOrDefault(s => s.LanguageCode == c.LanguageCode).Description = c.Description;
                }
                else
                {
                    category.Translations.Add(new EATranslation
                    {
                        LanguageCode = c.LanguageCode,
                        Title = c.Title,
                        Description = c.Description
                    });
                }
            });

            category.Translations.ForEach(c =>
            {
                if (@event.Translations.Any(s => s.LanguageCode == c.LanguageCode))
                {
                    c.IsDeleted = true;
                }
            });

            await _categoryRepository.Attach(category);
        }

        public async Task On(AddProductEvent @event)
        {
            if(@event.ProductId == Guid.Empty)
                throw new InvalidDataException("Incorrect Product Id is sended!");
            
            if(@event.ProductSystemId == Guid.Empty)
                throw new InvalidDataException("Incorrect System Product Id is sended!");
            
            
            
            
        }
    }
}
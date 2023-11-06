using EA.Domain.Entities;
using EA.Domain.Events;
using EA.Domain.Primitives.Entities;
using EA.Infrastructure;
using EAnalytics.Common.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EA.Parser.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(AddCategoryEvent @event);
        Task On(AddProductEvent @event);
    }

    public class EventHandler : IEventHandler
    {
        private readonly IRepository<Category, EADbContext> _categoryRepository;
        private readonly IRepository<Product, EADbContext> _productRepository;

        public EventHandler(IRepository<Category, EADbContext> categoryRepository, IRepository<Product, EADbContext> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public Task On(AddCategoryEvent @event)
        {
            var category = new Category()
            {
                Id = @event.Id,
                Translations = @event.Translations.Select(s => new EACategoryTranslation
                {
                    LanguageCode = s.LanguageCode,
                    Description = s.Description,
                    Title = s.Title
                }).ToList()
            };

            return _categoryRepository.CreateAsync(category);
        }

        public Task On(EditCategoryEvent @event)
        {
            if (@event.CategoryId == Guid.Empty)
                throw new InvalidDataException("Incorrect category Id is sended!");

            var category = _categoryRepository.Get(s => s.Id == @event.CategoryId, i => i.Include(s => s.Translations));

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
                    category.Translations.Add(new EACategoryTranslation
                    {
                        LanguageCode = c.LanguageCode,
                        Title = c.Title,
                        Description = c.Description
                    });
                }
            });

            category.Translations.ForEach(c =>
            {
                if (@event.Translations.Any(s => s.LanguageCode != c.LanguageCode))
                {
                    c.IsDeleted = true;
                }
            });

            _categoryRepository.Update(category);

            return Task.CompletedTask;
        }

        public Task On(AddProductEvent @event)
        {
            if (@event.ProductId == Guid.Empty)
                throw new InvalidDataException("Incorrect Product Id is sended!");

            if (@event.ProductSystemId == Guid.Empty)
                throw new InvalidDataException("Incorrect System Product Id is sended!");

            var product = new Product
            {
                Id = @event.ProductId,
                CategoryId = @event.Id,
                SystemProducts = new List<SystemProduct>
                {
                    new SystemProduct
                    {
                        Id = @event.ProductSystemId,
                        Name = @event.Name,
                        Price = @event.Price,
                        SystemName = @event.SystemName,
                        Url = @event.Url
                    }
                },

            };

            return _productRepository.CreateAsync(product);
        }
    }
}
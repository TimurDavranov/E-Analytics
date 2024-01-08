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
        Task On(UpdateOLCategoryEvent @event);
        Task On(EnableOLCategoryEvent @event);
        
        Task On(AddOLProductEvent @event);
        Task On(UpdateOlProductEvent @event);
    }

    public class EventHandler(
        IRepository<OLCategory, OLDbContext> categoryRepository,
        IRepository<OLProduct, OLDbContext> productRepository)
        : IEventHandler
    {

        public Task On(AddOLCategoryEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            if (@event.ParentId is 0)
                throw new InvalidDataException("Incorrect Parrent Id is sended!");

            return categoryRepository.CreateAsync(new OLCategory
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

        public Task On(UpdateOLCategoryEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            if (@event.ParentId is 0)
                throw new InvalidDataException("Incorrect Parrent Id is sended!");

            var category = categoryRepository.Get(s => s.SystemId == @event.SystemId, i => i.Include(s => s.Translations));

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
                return categoryRepository.UpdateAsync(category);
            }

            return Task.CompletedTask;
        }

        public async Task On(EnableOLCategoryEvent @event)
        {
            var category = await categoryRepository.GetAsync(s => s.Id == @event.Id);
            category.Enabled = @event.Enable;
            await categoryRepository.UpdateAsync(category);
        }

        public Task On(AddOLProductEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            return productRepository.CreateAsync(new OLProduct()
            {
                Id = @event.Id,
                Translations = @event.Translations.Select(s => new OLTranslation
                {
                    Title = s.Title,
                    Description = s.Description,
                    LanguageCode = s.LanguageCode.Code
                }).ToList(),
                SystemId = @event.SystemId,
                Price = @event.Price,
                InstalmentMaxMouth = @event.InstalmentMaxMouth,
                InstalmentMonthlyRepayment = @event.InstalmentMonthlyRepayment
            });
        }

        public Task On(UpdateOlProductEvent @event)
        {
            var category = productRepository.Get(s => s.Id == @event.Id, i => i.Include(s => s.Translations));

            if (category is not null)
            {
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
                    else if (category.Translations.All(s => s.LanguageCode != lang) && @event.Translations.Any(s => s.LanguageCode.Equal(lang)))
                    {
                        category.Translations.Add(new OLTranslation
                        {
                            LanguageCode = @event.Translations.FirstOrDefault(s => s.LanguageCode.Equal(lang))!.LanguageCode.Code,
                            Title = @event.Translations.FirstOrDefault(s => s.LanguageCode.Equal(lang))!.Title,
                            Description = string.Empty
                        });
                    }
                }

                category.Price = @event.Price;
                category.InstalmentMaxMouth = @event.InstalmentMaxMouth;
                category.InstalmentMonthlyRepayment = @event.InstalmentMonthlyRepayment;
                return productRepository.UpdateAsync(category);
            }

            return Task.CompletedTask;
        }
        
        
    }
}

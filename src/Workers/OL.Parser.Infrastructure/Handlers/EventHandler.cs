using EAnalytics.Common;
using EAnalytics.Common.Abstractions.Repositories;
using EAnalytics.Common.Services;
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

        public async Task On(UpdateOLCategoryEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            if (@event.ParentId is 0)
                throw new InvalidDataException("Incorrect Parrent Id is sended!");

            var category = await categoryRepository.GetAsync(s => s.SystemId == @event.SystemId,
                i => i.Include(s => s.Translations));

            if (category is not null)
            {
                category.ParrentId = @event.ParentId;
                category.SystemId = @event.SystemId;
                foreach (var lang in SupportedLanguageCodes.Codes)
                {
                    if (category.Translations.Any(s =>
                            s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase)) &&
                        @event.Translations.Any(
                            s => s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        category.Translations
                                .FirstOrDefault(s =>
                                    s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Title =
                            @event.Translations.FirstOrDefault(s =>
                                s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase)).Title;

                        category.Translations
                                .FirstOrDefault(s =>
                                    s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Description =
                            @event.Translations.FirstOrDefault(s =>
                                    s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Description;

                        continue;
                    }

                    if (category.Translations.All(s =>
                            !s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase)) &&
                        @event.Translations.Any(
                            s => s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var model = @event.Translations.FirstOrDefault(
                            s => s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase));
                        category.Translations.Add(new OLTranslation()
                        {
                            LanguageCode = model.LanguageCode.Code,
                            Title = model.Title,
                            Description = model.Description
                        });

                        continue;
                    }

                    if (category.Translations.Any(s =>
                            s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase)) &&
                        @event.Translations.All(
                            s => !s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        category.Translations
                            .FirstOrDefault(s =>
                                s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                            .IsDeleted = true;
                        continue;
                    }
                }

                await categoryRepository.UpdateAsync(category);
            }
        }

        public async Task On(EnableOLCategoryEvent @event)
        {
            var category = await categoryRepository.GetAsync(s => s.Id == @event.Id);
            category.Enabled = @event.Enable;
            await categoryRepository.UpdateAsync(category);
        }

        public async Task On(AddOLProductEvent @event)
        {
            if (@event.SystemId is 0)
                throw new InvalidDataException("Incorrect System Id is sended!");

            await productRepository.CreateAsync(new OLProduct()
            {
                Id = @event.Id,
                Translations = @event.Translations.Select(s => new OLTranslation
                {
                    Title = s.Title,
                    Description = s.Description,
                    LanguageCode = s.LanguageCode.Code
                }).ToList(),
                SystemId = @event.SystemId,
                Price = new List<OLProductPriceHistory>()
                {
                    new OLProductPriceHistory()
                    {
                        Date = DateTime.Now,
                        Price = @event.Price
                    }
                },
                InstalmentMaxMouth = @event.InstalmentMaxMouth,
                InstalmentMonthlyRepayment = @event.InstalmentMonthlyRepayment
            });
        }

        public async Task On(UpdateOlProductEvent @event)
        {
            var product = await productRepository.GetAsync(s => s.Id == @event.Id,
                i => i.Include(s => s.Translations).Include(s => s.Price));

            if (product is not null)
            {
                foreach (var lang in SupportedLanguageCodes.Codes)
                {
                    if (product.Translations.Any(s =>
                            s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase) &&
                            @event.Translations.Any(s =>
                                s.LanguageCode.Code.Equals(lang,
                                    StringComparison.InvariantCultureIgnoreCase))))
                    {
                        product.Translations
                                .FirstOrDefault(s =>
                                    s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Title =
                            @event.Translations
                                .FirstOrDefault(
                                    s => s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Title;

                        product.Translations
                                .FirstOrDefault(s =>
                                    s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Description =
                            @event.Translations
                                .FirstOrDefault(
                                    s => s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                                .Description;

                        continue;
                    }

                    if (product.Translations.All(s =>
                            !s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase) &&
                            @event.Translations.Any(s =>
                                s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        var model = @event.Translations.FirstOrDefault(s =>
                            s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase));
                        product.Translations.Add(new OLTranslation()
                        {
                            LanguageCode = model.LanguageCode.Code,
                            Title = model.Title,
                            Description = model.Description
                        });
                        continue;
                    }

                    if (product.Translations.Any(s =>
                            s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase) &&
                            @event.Translations.All(s =>
                                !s.LanguageCode.Code.Equals(lang, StringComparison.InvariantCultureIgnoreCase))))
                    {
                        product.Translations.FirstOrDefault(s =>
                            s.LanguageCode.Equals(lang, StringComparison.InvariantCultureIgnoreCase)).IsDeleted = true;
                    }
                }

                if (product.Price.MaxBy(s => s.Date)?.Price != @event.Price)
                    product.Price.Add(new OLProductPriceHistory()
                    {
                        Date = DateTime.Now,
                        Price = @event.Price
                    });

                product.InstalmentMaxMouth = @event.InstalmentMaxMouth;
                product.InstalmentMonthlyRepayment = @event.InstalmentMonthlyRepayment;
                await productRepository.UpdateAsync(product);
            }
        }
    }
}
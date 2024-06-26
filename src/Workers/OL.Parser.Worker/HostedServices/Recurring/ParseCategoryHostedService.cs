using System.Globalization;
using EAnalytics.Common;
using EAnalytics.Common.Dtos;
using OL.Domain.Dtos.Responces;
using OL.Infrastructure.Commands.Categories;
using OL.Infrastructure.Models.Requests.Category;
using OL.Infrastructure.Models.Responses.Category;
using OL.Parser.Worker.Services;

namespace OL.Parser.Worker.HostedServices.Recurring
{
    public class ParseCategoryHostedService(IServiceProvider provider, ILogger<ParseCategoryHostedService> logger)
        : IHostedService, IDisposable
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(3000);
                    logger.LogInformation("OL system category parsing is started at: {Date}",
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    var scope = provider.CreateScope();
                    var olSystemService = scope.ServiceProvider.GetRequiredService<OLSystemService>();
                    var categoryCommandService = scope.ServiceProvider.GetRequiredService<CategoryCommandService>();
                    var categoryQueryService = scope.ServiceProvider.GetRequiredService<CategoryQueryService>();

                    var categories = await olSystemService.GetCategories();

                    if (categories is not null &&
                        categories.Status.ToLowerInvariant() is "ok" &&
                        categories?.Data?.Categories is not null &&
                        categories.Data.Categories.Any())
                    {
                        var splitedCategories = SplitByChilds(categories.Data.Categories.ToList()).DistinctBy(s => s.Id)
                            .ToArray().AsReadOnly();
                        var parallelOption = new ParallelOptions()
                        {
                            CancellationToken = cancellationToken,
                            MaxDegreeOfParallelism = 5
                        };
                        var existedCategories = await categoryQueryService.GetBySystemIds(
                            new CategoryBySystemIdsRequest()
                            {
                                SystemIds = splitedCategories.Select(s => s.Id).ToArray()
                            });
                        await Parallel.ForEachAsync(splitedCategories, parallelOption, async (category, token) =>
                        {
                            var existedCategory = existedCategories.Data.FirstOrDefault(s => s.SystemId == category.Id);

                            if (existedCategory is null)
                                await categoryCommandService.AddOlCategoryCommand(new AddOlCategoryCommand
                                {
                                    SystemId = category.Id,
                                    SystemImageUrl = category.MainImage,
                                    Translations = new List<TranslationDto>
                                    {
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                                            Description = string.Empty,
                                            Title = category.NameOz ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                                            Description = string.Empty,
                                            Title = category.NameRu ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.EN),
                                            Description = string.Empty,
                                            Title = category.NameEn ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                            Description = string.Empty,
                                            Title = category.NameUz ?? string.Empty
                                        }
                                    },
                                    ParentId = category.ParentId
                                });
                            else if (!MatchCategories(existedCategory, category))
                                await categoryCommandService.UpdateOlCategoryCommand(new UpdateOlCategoryCommand
                                {
                                    Id = existedCategory.Id,
                                    ParentId = category.ParentId,
                                    SystemId = category.Id,
                                    SystemImageUrl = category.MainImage,
                                    Translations = new List<TranslationDto>
                                    {
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                                            Description = string.Empty,
                                            Title = category.NameOz ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                                            Description = string.Empty,
                                            Title = category.NameRu ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.EN),
                                            Description = string.Empty,
                                            Title = category.NameEn ?? string.Empty
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                            Description = string.Empty,
                                            Title = category.NameUz ?? string.Empty
                                        }
                                    }
                                });
                        });
                        logger.LogInformation("OL system category parsing is end at: {Date}",
                            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                    else if (categories?.Data?.Categories is null || !categories.Data.Categories.Any())
                    {
                        logger.LogError("OL system categries is empty. Time: {Date}",
                            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                    else
                    {
                        logger.LogError(
                            "OL system category parsing is finished with error message: {Message}, at {Date}",
                            categories?.Message ?? "Error message not parsed",
                            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }

                    await Task.Delay(TimeSpan.FromHours(12), cancellationToken);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogWarning("Job with name: {JobName} is stopped", nameof(ParseCategoryHostedService));
            return Task.CompletedTask;
        }

        private IList<OLSystemCategory> SplitByChilds(IList<OLSystemCategory> categories)
        {
            var result = new List<OLSystemCategory>();
            categories.ToList().ForEach(category =>
            {
                if (category?.Children is not null && category.Children.Any())
                    result.AddRange(SplitByChilds(category.Children.ToList()));

                result.Add(category);
            });
            return result;
        }

        private bool MatchCategories(CategoryResponse finded, OLSystemCategory getted)
        {
            if (finded.SystemId != getted.Id)
                return false;

            if (finded.ParentId != getted.ParentId)
                return false;

            return MatchTranslations(finded.Translations, new List<TranslationDto>
            {
                new()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                    Description = string.Empty,
                    Title = getted.NameOz
                },
                new()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                    Description = string.Empty,
                    Title = getted.NameRu
                },
                new()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.EN),
                    Description = string.Empty,
                    Title = getted.NameEn
                },
                new()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                    Description = string.Empty,
                    Title = getted.NameUz
                }
            });
        }

        private bool MatchTranslations(IReadOnlyList<TranslationDto> finded, IReadOnlyList<TranslationDto> getted)
        {
            foreach (var item in getted)
            {
                if (!finded.Any(s => s.LanguageCode.Equal(item.LanguageCode.Code)))
                    return false;

                if (!finded.FirstOrDefault(s => s.LanguageCode.Code == item.LanguageCode.Code)?.Title.Equals(item.Title,
                        StringComparison.InvariantCultureIgnoreCase) ?? false)
                    return false;
            }

            return true;
        }

        public void Dispose()
        {
        }
    }
}
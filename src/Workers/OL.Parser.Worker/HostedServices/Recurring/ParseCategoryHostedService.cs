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
    public class ParseCategoryHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<ParseCategoryHostedService> _logger;
        private PeriodicTimer? _timer;

        public ParseCategoryHostedService(IServiceProvider provider, ILogger<ParseCategoryHostedService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("OL system category parsing is started at: {date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    var scope = _provider.CreateScope();
                    var olSystemService = scope.ServiceProvider.GetRequiredService<OLSystemService>();
                    var categoryCommandService = scope.ServiceProvider.GetRequiredService<CategoryCommandService>();
                    var categoryQueryService = scope.ServiceProvider.GetRequiredService<CategoryQueryService>();

                    var categories = await olSystemService.GetCategories();

                    if (categories is not null &&
                        categories.Status.ToLowerInvariant() is "ok" &&
                        categories?.Data?.Categories is not null &&
                        categories.Data.Categories.Any())
                    {

                        foreach (var category in categories.Data.Categories)
                        {
                            var existedCategory = await categoryQueryService.GetBySystemId(new CategoryBySystemIdRequest
                            {
                                SystemId = category.Id
                            });

                            if (existedCategory is null)
                                existedCategory = await categoryQueryService.GetByName(new CategoryByNameRequest
                                {
                                    Translations = new List<TranslationDto>
                                {
                                    new()
                                    {
                                        LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                                        Description = string.Empty,
                                        Title = category.NameOz
                                    },
                                    new()
                                    {
                                        LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                                        Description = string.Empty,
                                        Title = category.NameRu
                                    },
                                    new()
                                    {
                                        LanguageCode = new LanguageCode(SupportedLanguageCodes.EN),
                                        Description = string.Empty,
                                        Title = category.NameEn
                                    },
                                    new()
                                    {
                                        LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                        Description = string.Empty,
                                        Title = category.NameUz
                                    }
                                }
                                });

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
                                            Title = category.NameOz
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                                            Description = string.Empty,
                                            Title = category.NameRu
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                                            Description = string.Empty,
                                            Title = category.NameEn
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                            Description = string.Empty,
                                            Title = category.NameUz
                                        }
                                    },
                                    ParentId = category.ParentId
                                });
                            else if (MatchCategories(existedCategory, category))
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
                                            Title = category.NameOz
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                                            Description = string.Empty,
                                            Title = category.NameRu
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                                            Description = string.Empty,
                                            Title = category.NameEn
                                        },
                                        new()
                                        {
                                            LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                            Description = string.Empty,
                                            Title = category.NameUz
                                        }
                                    }
                                });

                        }

                        _logger.LogInformation("OL system category parsing is end at: {Date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                    else if (categories?.Data?.Categories is null || !categories.Data.Categories.Any())
                    {
                        _logger.LogError("OL system categries is empty. Time: {Date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                    else
                    {
                        _logger.LogError("OL system category parsing is finished with error message: {Message}, at {Date}", categories?.Message ?? "Error message not parsed", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                    await Task.Delay(10000, cancellationToken);
                }
            }, cancellationToken);
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
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
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

                if (!finded.FirstOrDefault(s => s.LanguageCode.Equal(item.LanguageCode.Code))!.Title.Equals(item.Title, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Job with name: {JobName} is stoped", nameof(ParseCategoryHostedService));
            return Task.CompletedTask;
        }
    }
}
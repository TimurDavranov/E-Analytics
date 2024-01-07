using EAnalytics.Common;
using EAnalytics.Common.Dtos;
using OL.Infrastructure.Commands.Product;
using OL.Infrastructure.Models.Requests.Product;
using OL.Parser.Worker.Services;

namespace OL.Parser.Worker.HostedServices.Recurring;

public class ParseProductHostedService(IServiceProvider provider, ILogger<ParseProductHostedService> logger) : IHostedService, IDisposable
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                logger.LogInformation("OL system product parsing is started at: {Date}",
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                var scope = provider.CreateScope();
                var olSystemService = scope.ServiceProvider.GetRequiredService<OLSystemService>();
                var categoryQueryService = scope.ServiceProvider.GetRequiredService<CategoryQueryService>();
                var productQueryService = scope.ServiceProvider.GetRequiredService<ProductQueryService>();
                var productCommandService = scope.ServiceProvider.GetRequiredService<ProductCommandService>();
                var categories = await categoryQueryService.GetAllIds();
                if (categories?.Data is not null && categories.Data.Any())
                {
                    var parallelOption = new ParallelOptions()
                    {
                        CancellationToken = cancellationToken,
                        MaxDegreeOfParallelism = 5
                    };
                    
                    foreach (var category in categories.Data)
                    {
                        var products = await olSystemService.GetProducts(category.SystemId);
                        if (products?.Data is not null && products.Status.ToLowerInvariant() is "ok" && products.Data.Products.Any())
                        {
                            for (var page = products.Data.Paginator.CurrentPage + 1;
                                 page <= products.Data.Paginator.LastPage;
                                 page++)
                            {
                                var source = products.Data.Products.DistinctBy(s => s.Id).ToArray();
                                await Parallel.ForEachAsync(source, parallelOption,
                                    async (product, token) =>
                                    {
                                        var existed = await productQueryService.GetBySystemId(
                                            new ProductBySystemIdRequest()
                                                { SystemId = product.Id });

                                        if (existed is null)
                                            await productCommandService.AddOlProductCommand(
                                                new AddOlProductCommand()
                                                {
                                                    Translations = new List<TranslationDto>()
                                                    {
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.UZ),
                                                            Description = product.ShortDescriptionOz,
                                                            Title = product.NameOz ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                                            Description = product.ShortDescriptionUz,
                                                            Title = product.NameUz ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.RU),
                                                            Description = product.ShortDescriptionRu,
                                                            Title = product.NameRu ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.EN),
                                                            Description = string.Empty,
                                                            Title = product.NameEn ?? string.Empty
                                                        }
                                                    },
                                                    SystemId = product.Id,
                                                    SystemImageUrl = product.Images.ToArray()
                                                });
                                        else if(false)
                                            await productCommandService.UpdateOlProductCommand(
                                                new UpdateOlProductCommand()
                                                {
                                                    Id = existed.Id,
                                                    Translations = new List<TranslationDto>()
                                                    {
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.UZ),
                                                            Description = product.ShortDescriptionOz,
                                                            Title = product.NameOz ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                                                            Description = product.ShortDescriptionUz,
                                                            Title = product.NameUz ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.RU),
                                                            Description = product.ShortDescriptionRu,
                                                            Title = product.NameRu ?? string.Empty
                                                        },
                                                        new()
                                                        {
                                                            LanguageCode =
                                                                new LanguageCode(SupportedLanguageCodes.EN),
                                                            Description = string.Empty,
                                                            Title = product.NameEn ?? string.Empty
                                                        }
                                                    },
                                                    SystemId = product.Id,
                                                    SystemImageUrl = product.Images.ToArray()
                                                });
                                    });

                                products = await olSystemService.GetProducts(category.SystemId, page);
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromHours(12), cancellationToken);
            }
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Job with name: {JobName} is stopped", nameof(ParseCategoryHostedService));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
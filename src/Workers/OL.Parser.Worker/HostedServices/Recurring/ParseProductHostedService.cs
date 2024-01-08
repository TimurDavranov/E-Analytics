using System.Globalization;
using EAnalytics.Common;
using EAnalytics.Common.Dtos;
using OL.Domain.Dtos.Responces;
using OL.Domain.Mappers;
using OL.Infrastructure.Commands.Product;
using OL.Infrastructure.Models.Requests.Product;
using OL.Infrastructure.Models.Responses.Product;
using OL.Parser.Worker.Services;

namespace OL.Parser.Worker.HostedServices.Recurring;

public class ParseProductHostedService(IServiceProvider provider, ILogger<ParseProductHostedService> logger) : IHostedService, IDisposable
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000);
                logger.LogInformation("OL system product parsing is started at: {Date}",
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                var scope = provider.CreateScope();
                var olSystemService = scope.ServiceProvider.GetRequiredService<OLSystemService>();
                var categoryQueryService = scope.ServiceProvider.GetRequiredService<CategoryQueryService>();
                var productQueryService = scope.ServiceProvider.GetRequiredService<ProductQueryService>();
                var productCommandService = scope.ServiceProvider.GetRequiredService<ProductCommandService>();
                try
                {
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
                            if (products?.Data is not null && products.Status.ToLowerInvariant() is "ok" &&
                                products.Data.Products.Any())
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

                                            int? instalmentMaxMouth = null; 
                                            if (int.TryParse(product.Plan?.MaxPeriod, NumberStyles.None,
                                                    CultureInfo.InvariantCulture, out var value))
                                                instalmentMaxMouth = value;

                                            decimal.TryParse(product.TotalPrice, NumberStyles.None,
                                                CultureInfo.InvariantCulture, out var price);

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
                                                        SystemImageUrl = product.Images.ToArray(),
                                                        Price = price,
                                                        InstalmentMaxMouth = instalmentMaxMouth ?? 0,
                                                        InstalmentMonthlyRepayment = product.MonthlyRepayment,
                                                        SystemCategoryId = category.SystemId
                                                    });
                                            else if (!MatchProduct(existed, product, price, instalmentMaxMouth))
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
                                                        SystemImageUrl = product.Images.ToArray(),
                                                        Price = price,
                                                        InstalmentMaxMouth = instalmentMaxMouth ?? 0,
                                                        InstalmentMonthlyRepayment = product.MonthlyRepayment,
                                                        SystemCategoryId = category.SystemId
                                                    });
                                        });

                                    products = await olSystemService.GetProducts(category.SystemId, page);
                                }
                            }
                        }

                        logger.LogInformation("OL system category parsing is end at: {Date}",
                            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    }
                }
                catch (Exception e)
                {
                    logger.LogError("Error on parse prodduct from OL system. Message: {message}", e.Message);
                }

                await Task.Delay(TimeSpan.FromHours(12), cancellationToken);
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public bool MatchProduct(ProductResponse oldProduct, OLSystemProduct newProduct, decimal price, int? instalmentMaxMouth = null)
    {
        if (oldProduct.Price != price)
            return false;
        if (oldProduct.InstalmentMonthlyRepayment != newProduct.MonthlyRepayment)
            return false;

        if (!instalmentMaxMouth.HasValue)
            return MatchTranslations(oldProduct.Translations.AsReadOnly(), new []
            {
                new TranslationDto()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ),
                    Description = newProduct.ShortDescriptionOz,
                    Title = newProduct.NameOz
                },
                new TranslationDto()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.RU),
                    Description = newProduct.ShortDescriptionRu,
                    Title = newProduct.NameRu
                },
                new TranslationDto()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.EN),
                    Description = string.Empty,
                    Title = newProduct.NameEn
                },
                new TranslationDto()
                {
                    LanguageCode = new LanguageCode(SupportedLanguageCodes.UZ_CYRL),
                    Description = newProduct.ShortDescriptionUz,
                    Title = newProduct.NameUz
                }
            });
        
        if (oldProduct.InstalmentMaxMouth != instalmentMaxMouth)
            return false;
        
        return true;
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning("Job with name: {JobName} is stopped", nameof(ParseCategoryHostedService));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
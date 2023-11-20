using EAnalytics.Common;
using EAnalytics.Common.Dtos;
using OL.Infrastructure.Commands.Categories;
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new PeriodicTimer(TimeSpan.FromHours(12));
            do
            {
                _logger.LogInformation("OL system category parsing is started at: {date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                var scope = _provider.CreateScope();
                var olSystemService = scope.ServiceProvider.GetRequiredService<OLSystemService>();
                var categoryCommandService = scope.ServiceProvider.GetRequiredService<CategoryCommandService>();

                var categories = await olSystemService.GetCategories();

                if (categories is not null &&
                    categories.Status.ToLowerInvariant() is "ok" &&
                    categories?.Data?.Categories is not null &&
                    categories.Data.Categories.Any())
                {

                    foreach (var category in categories.Data.Categories)
                    {
                        await categoryCommandService.AddOLCategoryCommand(new AddOLCategoryCommand
                        {
                            ParentId = category.ParentId,
                            SystemId = category.Id,
                            SystemImageUrl = category.MainImage,
                            Translations = new List<TranslationDto>
                            {
                                new TranslationDto
                                {
                                    LanguageCode = new LanguageCode("uz"),
                                    Description = string.Empty,
                                    Title = category.NameOz
                                },
                                new TranslationDto
                                {
                                    LanguageCode = new LanguageCode("ru"),
                                    Description = string.Empty,
                                    Title = category.NameRu
                                },
                                new TranslationDto
                                {
                                    LanguageCode = new LanguageCode("en"),
                                    Description = string.Empty,
                                    Title = category.NameEn
                                },
                                new TranslationDto
                                {
                                    LanguageCode = new LanguageCode("uz-cyrl"),
                                    Description = string.Empty,
                                    Title = category.NameUz
                                }
                            }
                        });

                        await Task.Delay(10000);
                    }

                    _logger.LogInformation("OL system category parsing is end at: {date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                }
                else if (categories?.Data?.Categories is null || !categories.Data.Categories.Any())
                {
                    _logger.LogError("OL system categries is empty. Time: {date}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                }
                else
                {
                    _logger.LogError("OL system category parsing is finished with error message: {message}, at {date}", categories?.Message ?? "Error message not parsed", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                }
            } while (await _timer.WaitForNextTickAsync(cancellationToken));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Job with name: {jobName} is stoped", nameof(ParseCategoryHostedService));
            return Task.CompletedTask;
        }
    }
}
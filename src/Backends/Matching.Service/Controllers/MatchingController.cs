using Learning.AI;
using Learning.AI.Classes.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OL.Domain;

namespace Matching.Service.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchingController : Controller
    {
        private readonly ILogger<MatchingController> _logger;
        private readonly IOLDbContext _context;

        public MatchingController(ILogger<MatchingController> logger, IOLDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Predict()
        {
            Func<Task<List<ProductData>>> Action = () => _context.Products
                            .AsNoTracking()
                            .AsSplitQuery()
                            .Include(s => s.Translations)
                            .SelectMany(s => s.Translations.Select(a => new { s.Id, a.Title }))
                            .Select(s => new ProductData()
                            {
                                ProductId = s.Id.ToString(),
                                Title = s.Title
                            })
                            .Where(s => !string.IsNullOrWhiteSpace(s.Title))
                            .ToListAsync();

            var title = "Настольный микрофон для компьютера и телефона Hoco L16 59776";
            await PredictExtension.Predict(new PredictRequest(title, ""), Action);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Train()
        {
            var source = await _context.Products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(s => s.Translations)
                .SelectMany(s => s.Translations.Select(a => new { s.Id, a.Title }))
                .Select(s => new ProductData()
                {
                    ProductId = s.Id.ToString(),
                    Title = s.Title
                })
                .Where(s => !string.IsNullOrWhiteSpace(s.Title))
                .ToListAsync();

            TrainExtension.TrainMLProduct(source, "");
            return Ok();
        }
    }
}
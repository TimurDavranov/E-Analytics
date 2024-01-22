using Learning.AI;
using Learning.AI.Classes.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OL.Domain;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Matching.Service.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchingController : ControllerBase
    {
        private readonly ILogger<MatchingController> _logger;
        private readonly IOLDbContext _context;
        private readonly IConfiguration _configuration;
        private static Func<string, Task<List<ProductData>>> Act = async (string connectionString) =>
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var totalCount = await connection.QuerySingleAsync<long>(SqlCommandConstants.countProductItems);
            var source = await connection.QueryAsync<ProductData>(SqlCommandConstants.learningSourceSql);
            while(totalCount / 5 > source.Count())
                source = await connection.QueryAsync<ProductData>(SqlCommandConstants.learningSourceSql);

            await connection.CloseAsync();
            return source.ToList();
        };

        public MatchingController(ILogger<MatchingController> logger, IOLDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Predict(string title)
        {
            await _context.Predict(new PredictRequest(title, ""), Act, _configuration);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Train()
        {
            var source = await Act.Invoke(_configuration.GetConnectionString("DefaultConnection")!);
            var fileByte = TrainExtension.TrainMLProduct(source, "");
            return File(fileByte, "application/zip", "collections_download.zip");
        }
    }
}
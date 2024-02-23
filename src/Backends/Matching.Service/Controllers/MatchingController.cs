using Learning.AI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using EA.Infrastructure;
using EAnalytics.Common.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.Data;
using OL.Infrastructure;

namespace Matching.Service.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchingController : ControllerBase
    {
        private readonly ILogger<MatchingController> _logger;
        private readonly DatabaseContextFactory<OLDbContext> _olContextFactory;
        private readonly DatabaseContextFactory<EADbContext> _eaContextFactory;

        public MatchingController(ILogger<MatchingController> logger, DatabaseContextFactory<OLDbContext> olContextFactory, DatabaseContextFactory<EADbContext> eaContextFactory)
        {
            _logger = logger;
            _olContextFactory = olContextFactory;
            _eaContextFactory = eaContextFactory;
        }
        
        [HttpPost]
        public async Task<IActionResult> StartMatching()
        {
            await using var _olContext = _olContextFactory.CreateContext();
            await using var _eaContext = _eaContextFactory.CreateContext(); 
            var sourceQuery = _olContext.Products
                .AsNoTracking()
                .Where(s => !s.IsDeleted);
            
            return Ok();
        }
    }
    
    public class ProductData
    {
        [LoadColumn(0)]
        public string ProductId { get; set; }

        [LoadColumn(1)]
        public string ProductName { get; set; }
    }

    public class ProductPrediction
    {
        [ColumnName("PredictedLabel")]
        public string ProductId { get; set; }
    }
}
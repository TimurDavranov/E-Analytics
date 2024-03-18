using Microsoft.AspNetCore.Mvc;
using EA.Infrastructure;
using EAnalytics.Common.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using OL.Infrastructure;
using EA.Domain.Entities;
using EAnalytics.Common.Services;
using EAnalytics.Common.Constants;
using System.Data;

namespace Matching.Service.Controllers
{
    [Route("[controller]/[action]")]
    public class MatchingController : ControllerBase
    {
        private readonly ILogger<MatchingController> _logger;
        private readonly IMinioService _minioService;
        private readonly DatabaseContextFactory<OLDbContext> _olContextFactory;
        private readonly DatabaseContextFactory<EADbContext> _eaContextFactory;

        public MatchingController(ILogger<MatchingController> logger, DatabaseContextFactory<OLDbContext> olContextFactory, DatabaseContextFactory<EADbContext> eaContextFactory, IMinioService minioService)
        {
            _logger = logger;
            _olContextFactory = olContextFactory;
            _eaContextFactory = eaContextFactory;
            _minioService = minioService;
        }

        [HttpPost]
        public async Task<IActionResult> TrainProductSimilarityModel([FromBody] TimeSpan time)
        {
            byte[] existedModel;

            await using var olContext = _olContextFactory.CreateContext();
            await using var eaContext = _eaContextFactory.CreateContext();

            var olProducts = await olContext.Products
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .SelectMany(s => s.Translations.Select(a => new ProductData()
                {
                    ProductName = a.Title,
                    ProductId = s.Id.ToString()
                }))
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName))
                .ToArrayAsync();

            try
            {
                existedModel = await _minioService.Download(AppConstants.MLBucket, AppConstants.MLProductSimilarityModelName);
            }
            catch (FileNotFoundException)
            {
                existedModel = await FirstTrainProductSimilarityModel(olProducts);
            }

            existedModel = await RetrainProductSimilarityModel(olProducts, existedModel, time);

            return Ok();
        }

        private async Task<byte[]> FirstTrainProductSimilarityModel(ProductData[] products)
        {
            MLContext mlContext = new MLContext();

            IDataView dataView = mlContext.Data.LoadFromEnumerable(products);

            var dataPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ProductData.ProductName));

            var trainedModel = dataPipeline.Fit(dataView);

            //var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductPrediction>(trainedModel);

            return await UploadLearningModel(trainedModel, mlContext, dataView, AppConstants.MLBucket, AppConstants.MLProductSimilarityModelName);
        }

        private async Task<byte[]> RetrainProductSimilarityModel(ProductData[] products, byte[] modelByte, TimeSpan trainTime)
        {
            MLContext mlContext = new MLContext();

            using var ms = new MemoryStream(modelByte);
            ms.Position = 0;

            //var trainedModel = mlContext.Model.Load(ms, out var dataViewSchema);

            //IDataView dataView = mlContext.Data.LoadFromEnumerable(products);

            //trainedModel.Transform(dataView);

            //var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductPrediction>(trainedModel);

            var trainedModel = mlContext.Model.Load(ms, out var dataViewSchema);
            IDataView dataView = mlContext.Data.LoadFromEnumerable(products);
            var dataPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ProductData.ProductName));
            trainedModel = dataPipeline.Fit(dataView);

            return await UploadLearningModel(trainedModel, mlContext, dataView, AppConstants.MLBucket, AppConstants.MLProductSimilarityModelName);
        }

        private async Task<byte[]> UploadLearningModel(ITransformer trainedModel, MLContext mlContext, IDataView dataView, string bucket, string name)
        {
            using var ms = new MemoryStream();
            mlContext.Model.Save(trainedModel, dataView.Schema, ms);
            ms.Position = 0;

            await _minioService.Upload(bucket, name, "", ms, default, true);

            return ms.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> StartMatching()
        {
            await using var olContext = _olContextFactory.CreateContext();
            await using var eaContext = _eaContextFactory.CreateContext();

            var olProducts = await olContext.Products
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .SelectMany(s => s.Translations.Select(a => new ProductData()
                {
                    ProductName = a.Title,
                    ProductId = s.Id.ToString()
                }))
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName))
                .ToArrayAsync();

            var engine = GetEngine(olProducts);

            var result = new List<ProductSimilarity>();

            if (!eaContext.Products
                    .AsNoTracking()
                    .Include(s => s.SystemProducts)
                    .Any(s => !s.IsDeleted && s.SystemProducts != null && s.SystemProducts.Any()))
            {
                for (int i = 0; i < olProducts.Length; i++)
                {
                    foreach (var val in Match(engine, olProducts, olProducts[i]))
                        result.Add(val);
                }

                return Ok(result);
            }

            var eaProducts = await eaContext.Products
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .SelectMany(s =>
                    s.SystemProducts.Select(a => new ProductData
                    {
                        ProductName = a.Name,
                        ProductId = s.Id.ToString()
                    }))
                .Where(s => !string.IsNullOrWhiteSpace(s.ProductName))
                .ToArrayAsync();

            for (int i = 0; i < eaProducts.Length; i++)
                foreach (var val in Match(engine, olProducts, eaProducts[i]))
                    result.Add(val);

            return Ok(result);
        }

        private PredictionEngine<ProductData, ProductPrediction> GetEngine(ProductData[] products)
        {
            MLContext mlContext = new MLContext();

            IDataView dataView = mlContext.Data.LoadFromEnumerable(products);

            var dataPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ProductData.ProductName));

            var trainedModel = dataPipeline.Fit(dataView);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductData, ProductPrediction>(trainedModel);

            return predictionEngine;
        }

        private IEnumerable<ProductSimilarity> Match(PredictionEngine<ProductData, ProductPrediction> predictionEngine, ProductData[] products, ProductData product)
        {
            var searchDocument = new ProductData { ProductName = product.ProductName };
            var transformedSearchQuery = predictionEngine.Predict(searchDocument);

            for (int i = 0; i < products.Length; i++)
                products[i].Features = predictionEngine.Predict(new ProductData { ProductName = products[i].ProductName }).Features;

            var minSimilarityScore = 0.9f;

            var result = new List<ValueTuple<string, string, float>>();

            foreach (var item in products.Where(s => !s.ProductId.Equals(product.ProductId, StringComparison.InvariantCultureIgnoreCase) && s.ProductName.Equals(product.ProductName, StringComparison.InvariantCultureIgnoreCase)))
            {
                var similarityScore = CalculateCosineSimilarity(transformedSearchQuery.Features, item.Features);
                if (similarityScore < minSimilarityScore)
                    continue;
                yield return new ProductSimilarity(product.ProductId, new ProductSimilarityScores(item.ProductId, similarityScore));
            }
        }

        static float CalculateCosineSimilarity(float[] vector1, float[] vector2)
        {
            if (vector1 is null || vector2 is null) return 0.0f;

            float dotProduct = 0;
            float norm1 = 0;
            float norm2 = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                norm1 += vector1[i] * vector1[i];
                norm2 += vector2[i] * vector2[i];
            }
            return dotProduct / (float)(Math.Sqrt(norm1) * Math.Sqrt(norm2));
        }
    }

    public class ProductData
    {
        [LoadColumn(0)]
        public string ProductId { get; set; }

        [LoadColumn(1)]
        public string ProductName { get; set; }

        [ColumnName("Features")]
        public float[] Features { get; set; }
    }

    public class ProductPrediction
    {
        [ColumnName("Features")]
        public float[] Features { get; set; }
    }

    public record ProductSimilarityResult(string productId, ProductSimilarityScores[] similarities);
    public record ProductSimilarity(string productId, ProductSimilarityScores similarity);
    public record ProductSimilarityScores(string productId, float score);
}
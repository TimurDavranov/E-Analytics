using Learning.AI.Classes.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using OL.Domain;

namespace Learning.AI
{
    public record PredictRequest(string inputTitle, string path);
    public record PredictResult(string productId, float persectage);
    public static class PredictExtension
    {
        private static string _path = "";
        private const short maxSimilarity = 70;
        private static ITransformer _model;
        private static MLContext _context = new MLContext();


        public static async Task<PredictResult[]> Predict(this IOLDbContext context, PredictRequest request, Func<string, Task<List<ProductData>>> actionLoadSource, IConfiguration configuration)
        {
            _path = request.path;

            if (_model is null)
                _model = await LoadLearnedModel(actionLoadSource, configuration);


            var predictionEngine = _context.Model.CreatePredictionEngine<ProductData, ProductPrediction>(_model);
            var result = new List<PredictResult>();
            var source = await context.Products.AsNoTracking().Include(s => s.Translations).SelectMany(s => s.Translations.Select(a => new { a.Title, ProductId = s.Id.ToString() })).ToArrayAsync();
            foreach (var item in source)
            {
                var prediction = predictionEngine.Predict(new ProductData
                {
                    Title = item.Title,
                    PredicateTitle = request.inputTitle
                });

                if (maxSimilarity < prediction.Score * 100 && result.Exists(s => s.productId == item.ProductId))
                    result.Add(new PredictResult(item.ProductId.ToString(), prediction.Score));
            }

            return result.ToArray();
        }

        private static int counter = 1;
        private static List<ProductData>? source;
        private static async Task<ITransformer> LoadLearnedModel(Func<string, Task<List<ProductData>>> actionLoadSource, IConfiguration configuration)
        {
            if (counter == 10)
                throw new Exception("Fail on train/load learning model");

            try
            {
                ITransformer transformer;
                using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    transformer = _context.Model.Load(fs, out _);

                counter = 1;
                return transformer;
            }
            catch
            {
                if (source is null || !source.Any())
                    source = await actionLoadSource.Invoke(configuration.GetConnectionString("DefaultConnection")!);
                TrainExtension.TrainMLProduct(source, _path);
                counter++;
                return await LoadLearnedModel(actionLoadSource, configuration);
            }
        }
    }
}
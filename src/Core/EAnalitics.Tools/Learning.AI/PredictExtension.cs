using Learning.AI.Classes.Product;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Learning.AI
{
    public record PredictRequest(string inputTitle, string path);
    public record PredictResult(string title, float[]? persectage);
    public static class PredictExtension
    {
        private static string _path = "";

        private static ITransformer _model;
        private static MLContext _context = new MLContext();


        public static async Task<PredictResult> Predict(PredictRequest request, Func<Task<List<ProductData>>> actionLoadSource)
        {
            _path = request.path;

            if (_model is null)
                _model = await LoadLearnedModel(actionLoadSource);

            var predictionEngine = _context.Model.CreatePredictionEngine<ProductData, ProductPrediction>(_model);

            var prediction = predictionEngine.Predict(new ProductData { Title = request.inputTitle });

            return new PredictResult(prediction.Title, prediction.Score);
        }

        private static int counter = 1;
        private static List<ProductData>? source;
        private static async Task<ITransformer> LoadLearnedModel(Func<Task<List<ProductData>>> actionLoadSource)
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
                    source = await actionLoadSource.Invoke();
                TrainExtension.TrainMLProduct(source, _path);
                counter++;
                return await LoadLearnedModel(actionLoadSource);
            }
        }
    }
}
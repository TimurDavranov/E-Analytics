using Learning.AI.Classes.Product;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Learning.AI
{
    public static class TrainExtension
    {
        private static TransformerChain<KeyToValueMappingTransformer>? _model;
        private static IDataView? _testData;
        private static MLContext _context = new MLContext();
        private static string _path = "";

        public static void TrainMLProduct(List<ProductData> source, string path)
        {
            _path = path;

            var data = _context.Data.LoadFromEnumerable(source);

            var trainTestData = _context.Data.TrainTestSplit(data, 0.2);

            var trainData = trainTestData.TrainSet;

            _testData = trainTestData.TestSet;

            var pipeline = _context.Transforms.Text.FeaturizeText(Constants.feature, nameof(ProductData.Title))
                .Append(_context.Transforms.Conversion.MapValueToKey(Constants.label, nameof(ProductData.ProductId))
                .Append(_context.Transforms.Conversion.MapKeyToValue(Constants.predictedLabel, Constants.predictedLabel))
                .Append(_context.Transforms.Conversion.MapKeyToValue(nameof(ProductData.ProductId), nameof(ProductData.ProductId))));

            var trainer = _context.MulticlassClassification.Trainers.SdcaNonCalibrated();
            var trainingPipeline = pipeline.Append(trainer).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            _model = trainingPipeline.Fit(trainData);

            var predictions = _model!.Transform(_testData);

            var metrics = _context.MulticlassClassification.Evaluate(predictions);

            SaveLearnedModel();
        }

        private static void SaveLearnedModel()
        {
            using (var fs = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.Write))
                _context.Model.Save(_model, null, fs);
        }


    }
}
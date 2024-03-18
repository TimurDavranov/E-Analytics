using Learning.AI.Classes.Product;
using Microsoft.ML;
using Microsoft.ML.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Learning.AI
{
    public static class TrainExtension
    {
        private static TransformerChain<TransformerChain<ColumnConcatenatingTransformer>>? _model;
        private static MLContext _context = new MLContext();
        private static string _path = "";

        public static byte[] TrainMLProduct(List<ProductData> source, string path)
        {
            _path = path;

            var data = _context.Data.LoadFromEnumerable(source);

            var pipeline = BuildPipeline();

            _model = pipeline.Retrain();

            return SaveLearnedModel();
        }

        private static byte[] SaveLearnedModel()
        {
            using (var ms = new MemoryStream())
            {
                _context.Model.Save(_model, null, ms);
                return ms.ToArray();
            }
        }

        private static EstimatorChain<TransformerChain<ColumnConcatenatingTransformer>> BuildPipeline()
        {
            return _context.Transforms.Conversion.MapValueToKey(Constants.label, nameof(ProductData.Title))
                .Append(_context.Transforms.Text.FeaturizeText(Constants.titleFeaturized, nameof(ProductData.Title))
                .Append(_context.Transforms.Text.FeaturizeText(Constants.predicateTitleFeaturized, nameof(ProductData.PredicateTitle)))
                .Append(_context.Transforms.Concatenate(Constants.feature, Constants.titleFeaturized, Constants.predicateTitleFeaturized)));
        }


        private static TransformerChain<TransformerChain<ColumnConcatenatingTransformer>> Retrain(this EstimatorChain<TransformerChain<ColumnConcatenatingTransformer>> pipeline)
        {
            return pipeline.Fit(null);
        }

    }
}
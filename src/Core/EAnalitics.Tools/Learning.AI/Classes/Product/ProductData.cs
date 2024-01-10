using Microsoft.ML.Data;

namespace Learning.AI.Classes.Product
{
    public class ProductData
    {
        [LoadColumn(0)]
        public required string Title;
        [LoadColumn(1)]
        public string ProductId;
    }

    public class ProductPrediction
    {
        [ColumnName(Constants.predictedLabel)]
        public string Title;

        [ColumnName(Constants.score)]
        public float[]? Score;
    }
}
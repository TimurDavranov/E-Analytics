using Microsoft.ML.Data;

namespace Learning.AI.Classes.Product
{
    public class ProductData
    {
        [LoadColumn(0)]
        public required string Title;
        [LoadColumn(1)]
        public required string PredicateTitle;
    }

    public class ProductPrediction
    {
        [ColumnName(Constants.score)]
        public float Score;
    }
}
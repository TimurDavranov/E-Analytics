using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces
{
    public class OLCategoryResponse<T>
    {
        [JsonPropertyName("categories")]
        public ICollection<T> Categories { get; set; }
    }
}

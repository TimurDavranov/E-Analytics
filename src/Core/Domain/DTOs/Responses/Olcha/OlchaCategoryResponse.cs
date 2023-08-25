using System.Text.Json.Serialization;

namespace Domain.DTOs.Responses.Olcha
{
    public class OlchaCategoryResponse<T>
    {
        [JsonPropertyName("categories")]
        public ICollection<T> Categories { get; set; }
    }
}

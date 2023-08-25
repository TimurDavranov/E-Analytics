using System.Text.Json.Serialization;

namespace Domain.DTOs.Responses.Olcha
{
    public class OlchaResponse<T>
    {
        [JsonPropertyName("categories")]
        public ICollection<T> Categories { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces;

public class OLSystemCategoryData<T> where T : class
{
    [JsonPropertyName("categories")]
    public IReadOnlyList<T> Categories { get; init; }

    [JsonPropertyName("paginator")]
    public string? Paginator { get; init; }
}
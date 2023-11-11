using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces;

public class OLSystemManufacturer
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name_ru")]
    public string NameRu { get; init; }

    [JsonPropertyName("name_uz")]
    public string NameUz { get; init; }

    [JsonPropertyName("name_oz")]
    public string NameOz { get; init; }

    [JsonPropertyName("name_en")]
    public string NameEn { get; init; }

    [JsonPropertyName("slug")]
    public string Slug { get; init; }

    [JsonPropertyName("main_image")]
    public string MainImage { get; init; }

    [JsonPropertyName("isSelected")]
    public bool IsSelected { get; init; }
}

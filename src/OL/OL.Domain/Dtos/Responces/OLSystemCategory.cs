using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces;

public class OLSystemCategory
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("queue")]
    public int Queue { get; init; }

    [JsonPropertyName("parent_id")]
    public long? ParentId { get; init; }

    [JsonPropertyName("name_ru")]
    public string NameRu { get; init; }

    [JsonPropertyName("name_uz")]
    public string NameUz { get; init; }

    [JsonPropertyName("name_oz")]
    public string NameOz { get; init; }

    [JsonPropertyName("name_en")]
    public string NameEn { get; init; }

    [JsonPropertyName("alias")]
    public string Alias { get; init; }

    [JsonPropertyName("link")]
    public string Link { get; init; }

    [JsonPropertyName("main_image")]
    public string MainImage { get; init; }

    [JsonPropertyName("icon")]
    public string Icon { get; init; }

    [JsonPropertyName("background_image")]
    public string BackgroundImage { get; init; }

    [JsonPropertyName("webp_image")]
    public string WebpImage { get; init; }

    [JsonPropertyName("position")]
    public string Position { get; init; }

    [JsonPropertyName("children")]
    public IReadOnlyList<OLSystemCategory> Children { get; init; }

    [JsonPropertyName("manufacturers")]
    public IReadOnlyList<OLSystemManufacturer> Manufacturers { get; init; }
}

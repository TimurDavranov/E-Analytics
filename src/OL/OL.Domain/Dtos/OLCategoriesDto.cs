using System.Text.Json.Serialization;
using EAnalytics.Common.Primitives.DTOs;

namespace OL.Domain.Dtos
{
    public class OLCategoriesDto : BaseDto<long>
    {
        [JsonPropertyName("queue")]
        public int Queue { get; set; }
        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }
        [JsonPropertyName("name_ru")]
        public string NameRu { get; set; }
        [JsonPropertyName("name_uz")]
        public string NameUz { get; set; }
        [JsonPropertyName("name_oz")]
        public string NameOz { get; set; }
        [JsonPropertyName("name_en")]
        public string NameEn { get; set; }
        [JsonPropertyName("alias")]
        public string Alias { get; set; }
        [JsonPropertyName("link")]
        public string Link { get; set; }
        [JsonPropertyName("main_image")]
        public string MainImage { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        [JsonPropertyName("background_image")]
        public string BackgroundImage { get; set; }
        [JsonPropertyName("webp_image")]
        public string WebpImage { get; set; }
        [JsonPropertyName("position")]
        public string Position { get; set; }
        [JsonPropertyName("children")]
        public ICollection<OLCategoriesDto> Children { get; set; }
        [JsonPropertyName("manufacturers")]
        public ICollection<OLManufactureDto> Manufacturers { get; set; }
    }
}

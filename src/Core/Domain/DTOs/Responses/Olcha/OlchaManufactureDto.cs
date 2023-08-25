using Domain.Primitives.DTOs;
using System.Text.Json.Serialization;

namespace Domain.DTOs.Responses.Olcha
{
    public class OlchaManufactureDto : BaseDto
    {
        [JsonPropertyName("name_ru")]
        public string NameRu { get; set; }
        [JsonPropertyName("name_uz")]
        public string NameUz { get; set; }
        [JsonPropertyName("name_oz")]
        public string NameOz { get; set; }
        [JsonPropertyName("name_en")]
        public string NameEn { get; set; }
        [JsonPropertyName("slug")]
        public string Slug { get; set; }
        [JsonPropertyName("main_image")]
        public string MainImage { get; set; }
        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; set; }
    }
}

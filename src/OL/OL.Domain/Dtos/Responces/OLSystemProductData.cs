using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces;

public class OLSystemProductData
{
    [JsonPropertyName("products")] public List<OLSystemProduct> Products { get; init; }
    [JsonPropertyName("paginator")] public OLSystemProductPaginator Paginator { get; init; }
}

public class OLSystemProduct
{
    [JsonPropertyName("id")] public long Id { get; init; }

    [JsonPropertyName("name_ru")] public string NameRu { get; init; }

    [JsonPropertyName("name_uz")] public string NameUz { get; init; }

    [JsonPropertyName("name_oz")] public string NameOz { get; init; }

    [JsonPropertyName("name_en")] public string NameEn { get; init; }

    [JsonPropertyName("short_description_ru")]
    public string ShortDescriptionRu { get; init; }

    [JsonPropertyName("short_description_uz")]
    public string ShortDescriptionUz { get; init; }

    [JsonPropertyName("short_description_oz")]
    public string ShortDescriptionOz { get; init; }

    [JsonPropertyName("status")] public int Status { get; init; }

    [JsonPropertyName("discount")] public int Discount { get; init; }

    [JsonPropertyName("discount_value")] public string DiscountValue { get; init; }

    [JsonPropertyName("discount_type")] public string DiscountType { get; init; }

    [JsonPropertyName("discount_price")] public int DiscountPrice { get; init; }

    [JsonPropertyName("images")] public string[] Images { get; init; }

    [JsonPropertyName("total_price")] public string TotalPrice { get; init; }

    [JsonPropertyName("store_id")] public int StoreId { get; init; }
}

public class OLSystemProductPaginator
{
    [JsonPropertyName("current_page")] public int CurrentPage { get; init; }
    [JsonPropertyName("from")] public int From { get; init; }
    [JsonPropertyName("last_page")] public int LastPage { get; init; }
    [JsonPropertyName("per_page")] public int PerPage { get; init; }
    [JsonPropertyName("to")] public int To { get; init; }
    [JsonPropertyName("total")] public int Total { get; init; }
}
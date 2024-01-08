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

    [JsonPropertyName("images")] public string[] Images { get; init; }
    [JsonPropertyName("total_price")] public string TotalPrice { get; init; }

    [JsonPropertyName("monthly_repayment")]
    public decimal MonthlyRepayment { get; init; }

    [JsonPropertyName("plan")] public OLSYstemProductPlan Plan { get; init; }
}

public class OLSystemProductPaginator
{
    [JsonPropertyName("current_page")] public int CurrentPage { get; init; }
    [JsonPropertyName("last_page")] public int LastPage { get; init; }
}

public class OLSYstemProductPlan
{
    [JsonPropertyName("max_period")] public string MaxPeriod { get; init; }
}
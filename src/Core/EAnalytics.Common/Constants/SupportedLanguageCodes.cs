namespace EAnalytics.Common;

public static class SupportedLanguageCodes
{
    public const string UZ = "uz"; 
    public const string RU = "ru"; 
    public const string EN = "en"; 
    public const string UZ_CYRL = "uz-cyrl";

    public static readonly IReadOnlyCollection<string> Codes = new string[4] { UZ, RU, EN, UZ_CYRL };
}

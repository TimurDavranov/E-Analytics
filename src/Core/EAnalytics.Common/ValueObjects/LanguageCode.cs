
namespace EAnalytics.Common;

public record LanguageCode
{
    public string Code { get; set; }

    public LanguageCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentNullException("Language code is empty!");

        if (!SupportedLanguageCodes.Codes.Any(s => s.ToLowerInvariant().Equals(code.ToLowerInvariant())))
            throw new NotSupportedException("Not supported language code!");

        this.Code = code.ToLowerInvariant();
    }

    public bool Equal(string code)
        => Code.Equals(code);
}

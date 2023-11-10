using System.Text.Json;
using System.Text.Json.Serialization;

namespace EAnalytics.Common;

public class LanguageCodeConverter : JsonConverter<LanguageCode>
{
    public override LanguageCode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var code = reader.GetString();
        if (string.IsNullOrWhiteSpace(code))
            return null;

        return new(code);
    }

    public override void Write(Utf8JsonWriter writer, LanguageCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Code);
    }
}

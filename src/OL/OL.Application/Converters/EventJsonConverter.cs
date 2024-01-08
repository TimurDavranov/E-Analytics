using System.Text.Json;
using System.Text.Json.Serialization;
using EAnalytics.Common.Events;
using OL.Domain;

namespace OL.Application;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type type)
    {
        return type.IsAssignableFrom(typeof(BaseEvent));
    }

    public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
        }

        if (!doc.RootElement.TryGetProperty("Type", out var type))
        {
            throw new JsonException("Could not detect the Type discriminator property!");
        }

        var typeDiscriminator = type.GetString();
        var json = doc.RootElement.GetRawText();

        return typeDiscriminator switch
        {
            nameof(AddOLCategoryEvent) => JsonSerializer.Deserialize<AddOLCategoryEvent>(json, options),
            nameof(EnableOLCategoryEvent) => JsonSerializer.Deserialize<EnableOLCategoryEvent>(json, options),
            nameof(UpdateOLCategoryEvent) => JsonSerializer.Deserialize<UpdateOLCategoryEvent>(json, options),
            nameof(AddOLProductEvent) => JsonSerializer.Deserialize<AddOLProductEvent>(json, options),
            nameof(UpdateOlProductEvent) => JsonSerializer.Deserialize<UpdateOlProductEvent>(json, options),
            _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
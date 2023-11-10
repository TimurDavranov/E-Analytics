using System.Text.Json.Serialization;

namespace OL.Domain.Dtos.Responces;

public class OLSystemRoot<T> where T : class
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; }

    [JsonPropertyName("data")]
    public T Data { get; init; }
}

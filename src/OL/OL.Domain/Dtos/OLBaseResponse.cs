using System.Text.Json.Serialization;

namespace OL.Domain.Dtos
{
    public class OLBaseResponse<T>
    {
        public OLBaseResponse(string message, string status, T data)
        {
            Message = message;
            Status = status;
            Data = data;
        }

        [JsonPropertyName("message")] public string Message { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("data")] public T Data { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Responses.Olcha
{
    public class OlchaBaseResponse<T>
    {
        public OlchaBaseResponse(string message, string status, T data)
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
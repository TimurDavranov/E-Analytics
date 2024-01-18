using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Identity.Infrastructure.Models
{
    public class AuthModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public DateTime ExpiresIn { get; set; }
        
        [JsonPropertyName("refresh_expires_in")]
        public DateTime? RefreshExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        
        [JsonPropertyName("session_state")]
        public DateTime? SessionState { get; set; }
        
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

    }
}

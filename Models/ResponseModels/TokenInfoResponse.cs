namespace TranslateExample.Models.ResponseModels
{
    using System;
    using System.Text.Json.Serialization;

    public class TokenInfoResponse
    {
        [JsonPropertyName("iamToken")]
        public string IamToken { get; set; }

        [JsonPropertyName("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}

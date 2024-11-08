using System.Text.Json.Serialization;

namespace TranslateExample.Models.RequestModels
{
    public class KeyApiRequest
    {
        [JsonPropertyName("yandexPassportOauthToken")]
        public string Token { get; set; }
    }
}
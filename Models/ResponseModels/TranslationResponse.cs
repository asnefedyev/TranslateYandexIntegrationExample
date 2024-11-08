using System.Text.Json.Serialization;

namespace Example.Models.ResponseModels
{
    public class TranslationResponse
    {
        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Example.Models.RequestModels
{
    public class TranslationRequest
    {
        [JsonPropertyName("sourceLanguageCode")]
        public string SourceLanguageCode { get; set; }

        [JsonPropertyName("targetLanguageCode")]
        public string TargetLanguageCode { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("texts")]
        public List<string> Texts { get; set; }

        [JsonPropertyName("folderId")]
        public string FolderId { get; set; }

        [JsonPropertyName("speller")]
        public bool Speller { get; set; }
    }
}

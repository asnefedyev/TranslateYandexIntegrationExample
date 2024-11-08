namespace TranslateExample.Models.AppModels
{
    public class WordDTO
    {
        public string Hash { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public string SourceText { get; set; }
        public string TargetText { get; set; }
    }
}
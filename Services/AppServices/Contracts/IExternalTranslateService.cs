
namespace TranslatorExample.Services.CustomBackend.Contracts
{
    public interface IExternalTranslateService
    {
        public Task<string> DoYandexTranslateAsync(string sourceLanguage, string targetLanguage, string sourceText);
        public Task<string> DoTranslateCacheAsync(string sourceLanguage, string targetLanguage, string sourceText);
    }
}
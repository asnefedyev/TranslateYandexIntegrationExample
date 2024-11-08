using TranslateExample.Models.Logic;

namespace TranslateExample.Services.AppServices.Contracts
{
    public interface IPhraseCache
    {
        public void AddPhrase(TranslateSavedModel phrase);
        public bool TryGetPhraseByHash(TranslateSavedModel phraseInput, out TranslateSavedModel? translate);
    }
}
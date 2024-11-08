using Microsoft.Extensions.Caching.Memory;
using TranslateExample.Models.Logic;
using TranslateExample.Services.AppServices.Contracts;

namespace ExampleSource;

public class PhraseMemoryCache : IPhraseCache
{
    private readonly MemoryCache _cache;
    private readonly int _cacheTimeout;

    public PhraseMemoryCache(IMemoryCache cache, IConfiguration configuration)
    {
        _cache = (MemoryCache)cache;
        _cacheTimeout = configuration.GetValue<int>("cachTimeout");
    }

    public void AddPhrase(TranslateSavedModel phrase)
    {
        AddPhrase(phrase, (short)_cacheTimeout);
    }

    private void AddPhrase(TranslateSavedModel phrase, short minutes)
    {
        _cache.Set(phrase.Hash, phrase, TimeSpan.FromMinutes(minutes));
    }

    public bool TryGetPhraseByHash(TranslateSavedModel phraseInput, out TranslateSavedModel? translate)
    {
        if (_cache.TryGetValue(phraseInput.Hash, out translate))
        {
            return true;
        }
        return false;
    }
}
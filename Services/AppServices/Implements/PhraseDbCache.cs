using AutoMapper;
using Newtonsoft.Json;
using TranslateExample.Models.AppModels;
using TranslateExample.Models.Logic;
using TranslateExample.Services.AppServices.Contracts;
using TranslateExample.Services.Data.Contracts;

namespace ExampleSource;

public class PhraseDbCache : IPhraseCache
{
    public readonly ICacheDataService _cacheDataService;
    public readonly IMapper _mapper;


    public PhraseDbCache(ICacheDataService cacheDataService, IMapper mapper)
    {
        _cacheDataService = cacheDataService;
        _mapper = mapper;
    }

    public void AddPhrase(TranslateSavedModel phrase)
    {
        _cacheDataService.WordAdd(_mapper.Map<WordDTO>(phrase));
    }

    public bool TryGetPhraseByHash(TranslateSavedModel phraseInput, out TranslateSavedModel? translate)
    {
        var word = _cacheDataService.GetWord(phraseInput.Hash);
        if (word == null)
        {
            translate = null;
            return false;
        }
        else
        {
            translate = _mapper.Map<TranslateSavedModel>(word);
            AddPhrase(translate);
            return true;
        }
    }
}
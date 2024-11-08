using Example.Contracts.WebClientContracts;
using Refit;
using System.Net.Http.Headers;
using TranslateExample.Models.Logic;
using TranslateExample.Services.AppServices.Contracts;
using TranslateExample.Services.Data.Contracts;
using TranslateExample.Services.Factories.Contracts;
using TranslateExample.Services.Factories.Implements;
using TranslatorExample.Services.CustomBackend.Contracts;

namespace TranslatorExample.Services.CustomData.Implements
{
    public class ExternalYandexTranslateServices : IExternalTranslateService
    {
        private const string YandexClient_CloudApi = "YandexClient_CloudApi";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IPhraseCache _cacheMemoryService;
        private readonly IPhraseCache _cacheDbService;
        private readonly ICacheUpdateService _cacheUpdateService;
        private readonly ITokenDataService _tokenDataService;

        public ExternalYandexTranslateServices(IHttpClientFactory httpClientFactory, IConfiguration configuration, 
            IPhraseCacheFactory cacheServiceFactory, ICacheUpdateService cacheUpdateService, ITokenDataService tokenDataService) 
        {
            _httpClient = httpClientFactory.CreateClient(YandexClient_CloudApi);
            _configuration = configuration;
            _cacheMemoryService = cacheServiceFactory.CreatePhraseCacheService(CacheTypeEnum.MEM);
            _cacheDbService = cacheServiceFactory.CreatePhraseCacheService(CacheTypeEnum.DB);
            _cacheUpdateService = cacheUpdateService;
            _tokenDataService = tokenDataService;
        }

        public async Task<string>DoYandexTranslateAsync(string sourceLanguage, string targetLanguage, string sourceText)
        {
            var bearerToken = _tokenDataService.GetTokenFromDb();
            if (bearerToken is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }
            else throw new Exception("Не получен bearer-token!");
            
            var service = RestService.For<IYandexRefitContract>(_httpClient);
            var result = await service.DoYandexTranslate(new Example.Models.RequestModels.TranslationRequest
            {
                FolderId = _configuration["folderId"],
                Format = _configuration["translateFormat"],
                SourceLanguageCode = sourceLanguage,
                TargetLanguageCode = targetLanguage,
                Speller = _configuration.GetValue<bool>("speller"),
                Texts = [sourceText]
            });
            return result.Translations.FirstOrDefault().Text;
        }

        public async Task<string> DoTranslateCacheAsync(string sourceLanguage, string targetLanguage, string sourceText)
        {
            TranslateSavedModel tr = new()
            {
                Count = 0,
                SourceText = sourceText,
                SourceLang = sourceLanguage,
                TargetLang = targetLanguage,
                Source = SourceCacheEnum.External
            };

            if (_cacheMemoryService.TryGetPhraseByHash(tr, out var translateMem))
            {
                return translateMem.TargetText;
            }
            else
            {
                if (_cacheDbService.TryGetPhraseByHash(tr, out var translateDb))
                {
                    return translateDb.TargetText;
                }
                else
                {
                    tr.TargetText = await DoYandexTranslateAsync(sourceLanguage, targetLanguage, sourceText);
                    _cacheDbService.AddPhrase(tr);
                    _cacheUpdateService.DoActualCacheState(memCacheRem: null, dbCacheRem: 1, dbBytes: tr.TargetText.Length + sourceText.Length,
                        memBytes: null, reqRem: 1);
                    return tr.TargetText;
                }
            }
        }
    }
}
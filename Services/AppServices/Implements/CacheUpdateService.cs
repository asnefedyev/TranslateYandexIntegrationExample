using Microsoft.Extensions.Caching.Memory;
using TranslateExample.Models.DTO;
using TranslateExample.Services.AppServices.Contracts;
using TranslateExample.Services.Data.Contracts;
using TranslateExample.Services.Factories.Contracts;
using TranslateExample.Services.Factories.Implements;

namespace TranslateExample.Services.AppServices.Implements
{
    public class CacheUpdateService : ICacheUpdateService
    {
        private readonly ICacheDataService _cacheDataService;
        private readonly IMemoryCache _memoryCache;
        private readonly IPhraseCache _cacheMemoryService;
        private readonly IPhraseCache _cacheMemService;

        public CacheUpdateService(IServiceProvider sp, IMemoryCache cache, IPhraseCacheFactory cacheServiceFactory)
        {
            _cacheDataService = sp.GetRequiredService<ICacheDataService>();
            _memoryCache = cache;
            _cacheMemService = cacheServiceFactory.CreatePhraseCacheService(CacheTypeEnum.MEM);
        }

        public void RefreshMemoryCache()
        {
            _cacheDataService.RefreshTop10Items();
            var words = _cacheDataService.GetTop10WordsWithDetail();
            foreach (var word in words)
            {
                _cacheMemService.AddPhrase(new Models.Logic.TranslateSavedModel
                {
                    Count = 0,
                    Source = Models.Logic.SourceCacheEnum.Internal,
                    SourceLang = word.SourceLang,
                    SourceText = word.SourceText,
                    TargetLang = word.TargetLang,
                    TargetText = word.TargetText,
                });
            }
        }

        public void DoActualCacheState(int? memCacheRem = null, int? dbCacheRem = null, int? dbBytes = null, int? memBytes = null, int? reqRem = null)
        {
            _cacheDataService.DoActualCacheState(memCacheRem, dbCacheRem, dbBytes, memBytes, reqRem);
        }

        public CacheStateDTO GetCacheState()
        {
            return _cacheDataService.GetCacheState();
        }
    }
}
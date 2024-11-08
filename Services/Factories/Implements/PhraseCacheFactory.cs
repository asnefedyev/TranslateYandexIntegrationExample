using ExampleSource;
using TranslateExample.Services.AppServices.Contracts;
using TranslateExample.Services.Factories.Contracts;

namespace TranslateExample.Services.Factories.Implements
{
    public class PhraseCacheFactory : IPhraseCacheFactory
    {
        private readonly PhraseMemoryCache _memoryCache;
        private readonly PhraseDbCache _dbCache;

        public PhraseCacheFactory(PhraseMemoryCache memoryCache, PhraseDbCache dbCache)
        {
            _memoryCache = memoryCache;
            _dbCache = dbCache;
        }

        public IPhraseCache CreatePhraseCacheService(CacheTypeEnum cacheType)
        {
            return cacheType switch
            {
                CacheTypeEnum.MEM => _memoryCache,
                CacheTypeEnum.DB => _dbCache,
                _ => throw new ArgumentException("Invalid cache type")
            };
        }
    }
}

using TranslateExample.Services.AppServices.Contracts;
using TranslateExample.Services.Factories.Implements;

namespace TranslateExample.Services.Factories.Contracts
{
    public interface IPhraseCacheFactory
    {
        public IPhraseCache CreatePhraseCacheService(CacheTypeEnum cacheType);
    }
}
using TranslateExample.Models.DTO;

namespace TranslateExample.Services.AppServices.Contracts
{
    public interface ICacheUpdateService
    {
        public void RefreshMemoryCache();
        public void DoActualCacheState(int? memCacheRem = null, int? dbCacheRem = null, int? dbBytes = null, int? memBytes = null, int? reqRem = null);
        public CacheStateDTO GetCacheState();
    }
}

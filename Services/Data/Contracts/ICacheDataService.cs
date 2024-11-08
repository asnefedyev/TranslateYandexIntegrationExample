using TranslateExample.Models.AppModels;
using TranslateExample.Models.DTO;

namespace TranslateExample.Services.Data.Contracts
{
    public interface ICacheDataService
    {
        public void WordAdd(WordDTO word);
        public WordDTO GetWord(string hash);
        public List<Top10HashDTO> GetTop10Words();
        public List<WordDTO> GetTop10WordsWithDetail();
        public void RefreshTop10Items();
        public void DoActualCacheState(int? memCacheRem = null, int? dbCacheRem = null, int? dbBytes = null, int? memBytes = null, int? reqRem = null);
        public CacheStateDTO GetCacheState();
    }
}
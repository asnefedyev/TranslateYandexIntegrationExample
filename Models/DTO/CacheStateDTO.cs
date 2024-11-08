namespace TranslateExample.Models.DTO
{
    public class CacheStateDTO
    {
        public int MemCacheRem { get; set; } = 0;
        public int DbCacheRem { get; set; } = 0;
        public long DbBytes { get; set; } = 0;
        public long MemBytes { get; set; } = 0;
        public int ReqRem { get; set; } = 0;
    }
}
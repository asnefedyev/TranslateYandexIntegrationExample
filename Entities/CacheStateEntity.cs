using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranslateExample.Entities
{
    [Table("cashe_state", Schema = "public")]
    public class CacheStateEntity
    {
        [Key]
        [Column("x_memcache_rem")]
        public int MemCacheRem { get; set; }

        [Column("x_dbcache_rem")]
        public int DbCacheRem { get; set; }

        [Column("x_db_bytes")]
        public long DbBytes { get; set; }

        [Column("x_mem_bytes")]
        public long MemBytes { get; set; }

        [Column("x_req_rem")]
        public int ReqRem { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranslateExample.Entities
{
    [Table("top10_hash", Schema = "public")]
    public class Top10HashEntity
    {
        [Key]
        [Column("hash")]
        public string Hash { get; set; }

        [Column("cnt")]
        public int Count { get; set; }
    }
}

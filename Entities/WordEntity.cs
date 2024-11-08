using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranslateExample.Entities
{
    [Table("word", Schema = "public")]
    public class WordEntity
    {
        [Key]
        [Column("hash")]
        public string Hash { get; set; }

        [Column("source_lang")]
        [Required]
        public string SourceLang { get; set; }

        [Column("target_lang")]
        [Required]
        public string TargetLang { get; set; }

        [Column("source_text")]
        [Required]
        public string SourceText { get; set; }

        [Column("target_text")]
        [Required]
        public string TargetText { get; set; }

        public virtual ICollection<EventEntity> Events { get; set; }
    }
}
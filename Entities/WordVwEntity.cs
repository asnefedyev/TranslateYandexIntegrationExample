using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranslateExample.Entities
{
    [Table("word_vw", Schema = "public")]
    public class WordVwEntity
    {
        [Key]
        [Column("hash")]
        public string Hash { get; set; }

        [Column("source_lang")]
        public string SourceLang { get; set; }

        [Column("target_lang")]
        public string TargetLang { get; set; }

        [Column("source_text")]
        public string SourceText { get; set; }

        [Column("target_text")]
        public string TargetText { get; set; }
    }
}
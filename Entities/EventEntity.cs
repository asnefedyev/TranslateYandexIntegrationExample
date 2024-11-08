using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranslateExample.Entities
{
    [Table("event", Schema = "public")]
    public class EventEntity
    {
        [Key]
        [Column("event_id")]
        public long EventId { get; set; }

        [Column("word_hash")]
        [Required]
        [ForeignKey("Word")]
        public string WordHash { get; set; }

        [Column("event_time")]
        [Required]
        public DateTime EventTime { get; set; }

        public virtual WordEntity Word { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using TranslateExample.Entities;
using TranslateExample.Models.DTO;

namespace Example.Services.Common.Implements.Contexts
{
    public class ExampleDbContext : DbContext
    {
        public DbSet<WordEntity> Words { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<Top10HashEntity> Top10Hashes { get; set; }
        public DbSet<WordVwEntity> WordsVw { get; set; }
        public DbSet<CacheStateEntity> CacheState { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<WordEntity>()
                .ToTable("word", "public");

            modelBuilder.Entity<EventEntity>()
                .ToTable("event", "public");

            modelBuilder.Entity<Top10HashEntity>()
                .ToTable("top10_hash", "public");

            modelBuilder.Entity<WordVwEntity>()
                .ToTable("word_vw", "public");

            
            modelBuilder.Entity<EventEntity>()
                 .HasOne(e => e.Word)
                 .WithMany(w => w.Events)
                 .HasForeignKey(e => e.WordHash)
                 .HasPrincipalKey(w => w.Hash);
            */
        }
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options) { }
    }
}
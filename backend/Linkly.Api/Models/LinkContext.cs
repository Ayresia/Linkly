using Microsoft.EntityFrameworkCore;

namespace Linkly.Api.Models 
{
    public class LinkContext : DbContext
    {
        public virtual DbSet<Link> Links { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Link>(
                el =>
                {
                    el.HasKey(l => l.Slug);
                    el.Property(l => l.Slug)
                        .HasColumnType("CHAR(5)")
                        .IsRequired(true);

                    /* 
                     * The column length can be around 2MB / 2048 chars,
                     * although this is really unlikely to happen,
                     * It still can happen.
                    */

                    el.Property(l => l.Url)
                        .HasColumnType("VARCHAR(2048)") 
                        .IsRequired(true);
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseSqlite("Data Source=Database.db;");
    }
}

using Microsoft.EntityFrameworkCore;

namespace TvMazeScraper.Api.EF;

public class MazeContext : DbContext
{
    public MazeContext(DbContextOptions<MazeContext> options)
        : base(options)
    {
    }

    public DbSet<ShowContent> Responses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShowContent>()
            .HasIndex(p => p.ExternalId)
            .IsUnique();
        
        modelBuilder.Entity<ShowContent>()
            .Property(p => p.Content)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
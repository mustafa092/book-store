using Microsoft.EntityFrameworkCore;

namespace persistence.Data;

public class ElmDbContext : DbContext
{
    public ElmDbContext()
    {
    }

    public ElmDbContext(DbContextOptions<ElmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; } = null!;
    public virtual DbSet<SchemaVersion> SchemaVersions { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SchemaVersion>(entity =>
        {
            entity.Property(e => e.Applied).HasColumnType("datetime");

            entity.Property(e => e.ScriptName).HasMaxLength(255);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElmDbContext).Assembly);
    }
}
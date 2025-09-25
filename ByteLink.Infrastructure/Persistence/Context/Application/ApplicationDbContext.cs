using ByteLink.Domain.Entities;
using ByteLink.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ByteLink.Infrastructure.Persistence.Context.Application;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options)
{
    public DbSet<Url> Urls { get; set; }
    public DbSet<UrlVisit> UrlVisits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UrlConfiguration());
        modelBuilder.ApplyConfiguration(new UrlVisitConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

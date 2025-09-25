using ByteLink.Domain.Entities;
using ByteLink.Domain.Exceptions;
using ByteLink.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ByteLink.Infrastructure.Persistence.Context.Tenant;

public class TenantDbContext(
    DbContextOptions<TenantDbContext> options,
    IConfiguration configuration
) : DbContext(options)
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUser> AdminUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = configuration.GetConnectionString("tenant-database")
                ?? throw new MissingConfigurationException("TenantConnection");

            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.41"));
        }
    }
}
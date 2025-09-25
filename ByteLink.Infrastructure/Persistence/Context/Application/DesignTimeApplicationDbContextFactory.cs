using ByteLink.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ByteLink.Infrastructure.Persistence.Context.Application;

public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IConfiguration _configuration;

    public DesignTimeApplicationDbContextFactory() : this(
            new ConfigurationBuilder()
#if DEBUG
                .AddJsonFile("appsettings.Development.json")
#else
                .AddJsonFile("appsettings.json")
#endif
                .Build())
    { }

    public DesignTimeApplicationDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var starterDatabaseConnectionString = _configuration.GetConnectionString("StarterDatabaseConnection")
            ?? throw new MissingConfigurationException("StarterDatabaseConnection");

        optionsBuilder.UseMySql(starterDatabaseConnectionString, ServerVersion.Parse("8.0.41"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

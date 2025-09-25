using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Context.Application;
using ByteLink.Infrastructure.Persistence.Context.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Infrastructure.Persistence.Repositories;

public interface ITenantRepository
{
    public Task CreateTenantUserDatabaseAsync(ApplicationUser user);
}

public class TenantRepository(
    TenantDbContext tenantDbContext,
    [FromKeyedServices(GeneratorKeyedServices.UserDatabaseConnectionStringGenerator)] IGenerator<ApplicationUser, string> userDatabaseStringGenerator
) : ITenantRepository
{
    public async Task CreateTenantUserDatabaseAsync(ApplicationUser user)
    {
        await tenantDbContext.Database.ExecuteSqlRawAsync($"CREATE USER '{user.DatabaseUser}'@'%' IDENTIFIED BY '{user.DatabasePWD}';");
        await tenantDbContext.Database.ExecuteSqlRawAsync($"CREATE DATABASE `{user.DatabaseName}`;");
        await tenantDbContext.Database.ExecuteSqlRawAsync(
            $"GRANT ALL PRIVILEGES ON `{user.DatabaseName}`.* TO '{user.DatabaseUser}'@'%'; FLUSH PRIVILEGES;");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var starterDatabaseConnectionString = userDatabaseStringGenerator.Generate(user);

        optionsBuilder.UseMySql(starterDatabaseConnectionString, ServerVersion.Parse("8.0.41"));

        var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options);
        await applicationDbContext.Database.MigrateAsync();
    }
}

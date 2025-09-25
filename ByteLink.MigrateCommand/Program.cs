using ByteLink.Application.Generators;
using ByteLink.Infrastructure.Persistence.Context.Application;
using ByteLink.Infrastructure.Persistence.Context.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
                .AddJsonFile("appsettings.Development.json")
#else
                .AddJsonFile("appsettings.json")
#endif
    .Build();


var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();

var tenantConnectionString = configuration.GetConnectionString("tenant-database");
var keys = new List<string>();

foreach (var kvp in configuration.AsEnumerable())
{
    keys.Add($"{kvp.Key} = {kvp.Value}");
}

optionsBuilder.UseMySql(tenantConnectionString, ServerVersion.Parse("8.0.41"));

var tenantDbContext = new TenantDbContext(optionsBuilder.Options, configuration);
var userDatabaseConnectionStringGenerator = new UserDatabaseConnectionStringGenerator(configuration);
var loggerFactory = LoggerFactory.Create(builder =>
{

    builder.SetMinimumLevel(LogLevel.Information);
    builder.AddConsole();
});

var logger = loggerFactory.CreateLogger("Database Migration Command");

logger.LogInformation("Tenant Database Migrations: Started");
try
{
    var pendingMigrationsBefore = await tenantDbContext.Database.GetPendingMigrationsAsync();

    if (pendingMigrationsBefore.Any())
    {
        await tenantDbContext.Database.MigrateAsync();
        var migrationsApplied = string.Join('\n', pendingMigrationsBefore);
        logger.LogInformation("Applied Tenant Database migrations:\n{migrationsApplied}", migrationsApplied);
    }
    else
    {
        logger.LogInformation("Tenant Database has no pending migrations. Skipping...");
    }
}catch(Exception ex)
{
    logger.LogError("{TimeStamp}: Exception for tenant database - {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}", DateTime.Now, ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine);
    File.AppendAllText($"migration_failure-tenantdb", $"{DateTime.Now} - Exception for tenant database - {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}");
    throw;
}
logger.LogInformation("Tenant Database Migrations: Completed");

logger.LogInformation("Application User Database Migrations: Started");
foreach (var user in await tenantDbContext.Users.ToListAsync())
{
    try
    {
        var applicationOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var applicationConnectionString = userDatabaseConnectionStringGenerator.Generate(user);

        applicationOptionsBuilder
            .UseLoggerFactory(loggerFactory)
            .UseMySql(applicationConnectionString, ServerVersion.Parse("8.0.41"), contextOptionsBuilder =>
            {
                contextOptionsBuilder.MigrationsAssembly("ByteLink.Infrastructure");
            });

        var applicationDbContext = new ApplicationDbContext(applicationOptionsBuilder.Options);

        var pendingMigrations = await applicationDbContext.Database.GetPendingMigrationsAsync();

        if (!pendingMigrations.Any())
        {
            logger.LogInformation("Database ({user.DatabaseName}) had no pending migrations. Skipping...", user.DatabaseName);
            continue;
        }

        var migrationsAppliedBefore = await applicationDbContext.Database.GetAppliedMigrationsAsync();
        await applicationDbContext.Database.MigrateAsync();
        var migrationsAppliedAfter = await applicationDbContext.Database.GetAppliedMigrationsAsync();

        var migrationDifference = migrationsAppliedAfter.Except(migrationsAppliedBefore);
        var migrationsApplied = string.Join('\n', migrationDifference);

        logger.LogInformation("Successfully applied migrations to database: {user.DatabaseName}.\n{migrationsApplied}", user.DatabaseName, migrationsApplied);
    }
    catch(Exception ex)
    {
        var errorDetails = $"{DateTime.Now}: Exception for user {user.Id} - {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
        logger.LogError("Failed to migrate database: {user.DatabaseName} to newest schema.\n{errorDetails}", user.DatabaseName, errorDetails);
        File.AppendAllText($"migration_failure-{user.DatabaseName}", errorDetails);
    }
}

logger.LogInformation("Application User Database Migrations: Completed");
Environment.Exit(0);
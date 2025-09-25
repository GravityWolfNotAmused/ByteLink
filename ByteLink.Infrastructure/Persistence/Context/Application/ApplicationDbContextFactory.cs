using ByteLink.Domain;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Context.Base;
using ByteLink.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Infrastructure.Persistence.Context.Application;

public class ApplicationDbContextFactory(
    IUserRepository userRepository,
    [FromKeyedServices(GeneratorKeyedServices.UserDatabaseConnectionStringGenerator)] IGenerator<ApplicationUser, string> userDatabaseConnectionStringGenerator
) : IAsyncDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContextWithSQLConnectionString(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.41"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    public ApplicationDbContext CreateDbContextWithUser(ApplicationUser user)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = userDatabaseConnectionStringGenerator.Generate(user);

        return CreateDbContextWithSQLConnectionString(connectionString);
    }

    public async Task<ApplicationDbContext> CreateDbContextAsync()
    {
        var tenantUser = await userRepository.GetAuthorizedUserAsync();
        return CreateDbContextWithUser(tenantUser);
    }
}
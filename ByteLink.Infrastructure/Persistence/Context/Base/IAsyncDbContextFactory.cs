using ByteLink.Domain.Entities;
using ByteLink.Infrastructure.Persistence.Context.Application;
using Microsoft.EntityFrameworkCore;

namespace ByteLink.Infrastructure.Persistence.Context.Base;

public interface IAsyncDbContextFactory<TContextType> where TContextType : DbContext
{
    public Task<TContextType> CreateDbContextAsync();
    public ApplicationDbContext CreateDbContextWithSQLConnectionString(string connectionString);
    public TContextType CreateDbContextWithUser(ApplicationUser user);
}

using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Context.Application;
using ByteLink.Infrastructure.Persistence.Context.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Infrastructure.Persistence.Repositories;

public interface IUrlVisitRepository
{
    public Task<bool> AddVisitAsync(Url url);
    public Task<List<UrlVisit>> GetVisitsAsync(Url url);
}

public class UrlVisitRepository(
    IAsyncDbContextFactory<ApplicationDbContext> contextFactory,
    [FromKeyedServices(GeneratorKeyedServices.UserIdGenerator)] IGenerator<string, long> userIdGenerator,
    IUserRepository userRepository
) : IUrlVisitRepository
{
    public async Task<bool> AddVisitAsync(Url url)
    {
        var userId = userIdGenerator.Generate(url.UserId);
        var user = await userRepository.GetUserByIdAsync(userId);
        var context = contextFactory.CreateDbContextWithUser(user);

        var visit = UrlVisit.Create(url.Id);

        await context.UrlVisits.AddAsync(visit);
        var rowsInserted = await context.SaveChangesAsync();

        return rowsInserted != 0;
    }

    public async Task<List<UrlVisit>> GetVisitsAsync(Url url)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var visits = await context.UrlVisits
            .Where(visit => visit.UrlId == url.Id)
            .ToListAsync();

        return visits ?? [];
    }
}

using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Exceptions;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Context.Application;
using ByteLink.Infrastructure.Persistence.Context.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace ByteLink.Infrastructure.Persistence.Repositories;

public interface IUrlRepository
{
    public Task<Url> GetUrlByShortCodeAsync(string shortCode);
    public Task<Url> GetUrlByShortCodeAndUserIdAsync(string userId, string shortCode);
    public Task<bool> ExistsAsync(string shortCode);
    public Task<bool> AddAsync(Url url);
    public Task<bool> DeleteUrlByShortCodeAsync(string shortCode);
    public Task<IEnumerable<Url>> GetUrlsAsync(string userSqid);
}

public class UrlRepository(
    IAsyncDbContextFactory<ApplicationDbContext> contextFactory,
    IMemoryCache cache,
    IUserRepository userRepository,
    [FromKeyedServices(GeneratorKeyedServices.UserIdGenerator)] IGenerator<string, long> userIdGenerator
) : IUrlRepository
{
    public async Task<bool> ExistsAsync(string shortCode)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.Urls.AnyAsync(url => url.ShortCode.ToLower() == shortCode.ToLower());
    }

    public async Task<bool> AddAsync(Url url)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.AddAsync(url);

        var rowsAdded = await context.SaveChangesAsync();

        return rowsAdded > 0;
    }

    public async Task<Url> GetUrlByShortCodeAsync(string shortCode)
    {
        var context = await contextFactory.CreateDbContextAsync();

        var url = await context.Urls
            .Where(url => url.ShortCode == shortCode)
            .FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(Url), shortCode);

        return url!;
    }

    public async Task<bool> DeleteUrlByShortCodeAsync(string shortCode)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var exists = await ExistsAsync(shortCode);

        if (!exists)
        {
            throw new NotFoundException(nameof(Url), shortCode);
        }

        var success = await context.Urls
            .Where(url => url.ShortCode == shortCode)
            .ExecuteDeleteAsync();

        return success > 0;
    }

    public async Task<Url> GetUrlByShortCodeAndUserIdAsync(string userSqid, string shortCode)
    {
        if (!cache.TryGetValue(shortCode, out ApplicationUser? user))
        {
            var userId = userIdGenerator.Generate(userSqid);
            user = await userRepository.GetUserByIdAsync(userId);
        }

        var context = contextFactory.CreateDbContextWithUser(user!);

        var url = await context.Urls
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode)
            ?? throw new NotFoundException(nameof(Url), shortCode);

        cache.Set(url.ShortCode, user!, TimeSpan.FromHours(1));

        return url;
    }

    public async Task<IEnumerable<Url>> GetUrlsAsync(string userSqid)
    {
        var user = await userRepository.GetUserBySqidAsync(userSqid);
        var context = contextFactory.CreateDbContextWithUser(user);

        var urls = await context.Urls
            .ToListAsync();

        return urls ?? [];
    }
}

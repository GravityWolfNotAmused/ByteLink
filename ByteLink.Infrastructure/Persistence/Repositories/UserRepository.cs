using ByteLink.Domain;
using ByteLink.Domain.Comparators;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Exceptions;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Context.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Infrastructure.Persistence.Repositories;

public interface IUserRepository
{
    public Task<ApplicationUser> GetAuthorizedUserAsync();
    public Task<ApplicationUser> GetAnonymouseUserAsync();
    public Task<ApplicationUser> GetUserAsync(string email);
    public Task<ApplicationUser> GetUserByIdAsync(long id);
    public Task<ApplicationUser> GetUserBySqidAsync(string userSqid);
    public Task<bool> ExistsAsync(string email);
    public Task<ApplicationUser?> CreateUserAsync(ApplicationUser user);
    public Task<string> LoginAsync(string email, string password);
}

public class UserRepository(
    TenantDbContext context,
    IApplicationHttpContext applicationHttpContext,
    [FromKeyedServices(GeneratorKeyedServices.JwtTokenGenerator)] IGenerator<string, string> jwtTokenGenerator,
    [FromKeyedServices(ComparatorKeyedServices.PasswordValidator)] IComparator<string> passwordComparator,
    [FromKeyedServices(GeneratorKeyedServices.UserSqidGenerator)] IGenerator<long, string> userSqidIdGenerator,
    [FromKeyedServices(GeneratorKeyedServices.UserIdGenerator)] IGenerator<string, long> userIdGenerator
) : IUserRepository
{
    public async Task<ApplicationUser?> CreateUserAsync(ApplicationUser user)
    {
        if (await ExistsAsync(user.Email)) throw new DuplicateUserException(user.Email);

        var databaseEntry = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var userEntity = databaseEntry.Entity;

        userEntity.UserId = userSqidIdGenerator.Generate(userEntity.Id);
        await context.SaveChangesAsync();
        return userEntity;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
    }

    public async Task<ApplicationUser> GetAuthorizedUserAsync()
    {
        var email = applicationHttpContext.GetAuthorizedEmail();

        var user = await context.Users
            .Where(user => user.Email.ToLower() == email.ToLower())
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(ApplicationUser), email);

        return user;
    }

    public async Task<ApplicationUser> GetUserAsync(string email)
    {
        var user = await context.Users
            .Where(user => user.Email.ToLower() == email.ToLower())
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(ApplicationUser), email);

        return user;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(long id)
    {
        var user = await context.Users
            .Where(user => user.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new NotFoundException(nameof(ApplicationUser), id);

        return user;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await GetUserAsync(email);
        var doesPasswordMatch = passwordComparator.Compare(password, user.PasswordHash);

        if (!doesPasswordMatch)
            throw new InvalidLoginException();

        return jwtTokenGenerator.Generate(user.Email);
    }

    public async Task<ApplicationUser> GetUserBySqidAsync(string userSqid)
    {
        var userId = userIdGenerator.Generate(userSqid);
        return await GetUserByIdAsync(userId);
    }

    public async Task<ApplicationUser> GetAnonymouseUserAsync()
    {
        return await GetUserBySqidAsync(Constants.AnonymousUserId);
    }
}

using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Application.Mediator.Handlers.AuthController;

public class RegisterUserHandler(
    IUserRepository userRepository,
    ITenantRepository tenantRepository,
    [FromKeyedServices(GeneratorKeyedServices.DatabasePwdGenerator)] IGenerator<string, string> databasePasswordGenerator,
    [FromKeyedServices(GeneratorKeyedServices.DatabaseUserNameGenerator)] IGenerator<string, string> databaseUserNameGenerator,
    [FromKeyedServices(GeneratorKeyedServices.DatabaseNameGenerator)] IGenerator<string, string> databaseNameGenerator,
    IMapper mapper
) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<ApplicationUser>(request);

        user.DatabaseUser = databaseUserNameGenerator.Generate(request.Email);
        user.DatabasePWD = databasePasswordGenerator.Generate(request.Email);
        user.DatabaseName = databaseNameGenerator.Generate(request.Email);

        await tenantRepository.CreateTenantUserDatabaseAsync(user);
        await userRepository.CreateUserAsync(user);
    }
}

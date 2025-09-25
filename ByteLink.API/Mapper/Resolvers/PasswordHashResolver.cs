using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;

namespace ByteLink.API.Mapper.Resolvers;

public class PasswordHashResolver(
    [FromKeyedServices(GeneratorKeyedServices.PasswordHashGenerator)] IGenerator<string, string> passwordHashGenerator
) : IValueResolver<RegisterUserCommand, ApplicationUser, string>
{
    public string Resolve(RegisterUserCommand source, ApplicationUser destination, string destMember, ResolutionContext context)
    {
        return passwordHashGenerator.Generate(source.Password);
    }
}

using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Application.Generators;

public class DatabasePwdGenerator(
    [FromKeyedServices(GeneratorKeyedServices.PasswordHashGenerator)] IGenerator<string, string> passwordHashGenerator
) : IGenerator<string, string>
{
    public string Generate(string input)
    {
        return passwordHashGenerator.Generate(input)[..16];
    }
}

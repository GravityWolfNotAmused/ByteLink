using ByteLink.Domain.Entities;
using ByteLink.Domain.Exceptions;
using ByteLink.Domain.Generators;
using Microsoft.Extensions.Configuration;

namespace ByteLink.Application.Generators;

public class UserDatabaseConnectionStringGenerator(
    IConfiguration configuration
) : IGenerator<ApplicationUser, string>
{
    public string Generate(ApplicationUser input)
    {
        var applicationConnectionString = configuration.GetConnectionString("ApplicationConnection")
            ?? throw new MissingConfigurationException("ApplicationConnection");

        return applicationConnectionString
            .Replace("@database", input.DatabaseName)
            .Replace("@user", input.DatabaseUser)
            .Replace("@pwd", input.DatabasePWD);
    }
}

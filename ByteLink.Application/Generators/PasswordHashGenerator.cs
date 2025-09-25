using ByteLink.Domain.Generators;

namespace ByteLink.Application.Generators;

public class PasswordHashGenerator : IGenerator<string, string>
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

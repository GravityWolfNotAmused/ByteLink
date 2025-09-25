using ByteLink.Domain.Generators;
using System.Security.Cryptography;
using System.Text;

namespace ByteLink.Application.Generators;

public class DatabaseNameGenerator : IGenerator<string, string>
{
    public string Generate(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(inputBytes);

        return string.Join("", hashBytes.Select(b => b.ToString("x2")));
    }
}

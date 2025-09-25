using ByteLink.Domain.Generators;
using System.Security.Cryptography;
using System.Text;

namespace ByteLink.Application.Generators;

public class ShortCodeGenerator : IGenerator<string, string>
{
    public string Generate(string url)
    {
        var inputBytes = Encoding.UTF8.GetBytes(url);
        var hashBytes = SHA1.HashData(inputBytes);

        return string.Join("", hashBytes.Select(b => b.ToString("x2")))[..10];
    }
}

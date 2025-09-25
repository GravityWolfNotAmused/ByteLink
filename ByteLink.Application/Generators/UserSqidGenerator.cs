using ByteLink.Domain.Generators;
using Sqids;

namespace ByteLink.Application.Generators;

public class UserSqidGenerator : IGenerator<long, string>
{
    public string Generate(long input)
    {
        var sqids = new SqidsEncoder<long>();
        return sqids.Encode(input);
    }
}

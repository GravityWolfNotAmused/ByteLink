using ByteLink.Domain.Generators;
using Sqids;

namespace ByteLink.Application.Generators;

public class UserIdGenerator : IGenerator<string, long>
{
    public long Generate(string input)
    {
        var sqids = new SqidsEncoder<long>();
        return sqids.Decode(input).Single();
    }
}

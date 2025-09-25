using ByteLink.Domain.Comparators;

namespace ByteLink.Application.Comparators;

public class PasswordValidator : IComparator<string>
{
    public bool Compare(string input, string storedValue)
    {
        return BCrypt.Net.BCrypt.Verify(input, storedValue);
    }
}
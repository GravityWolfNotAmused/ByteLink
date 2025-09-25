namespace ByteLink.Domain.Comparators;

public interface IComparator<TInput>
{
    public bool Compare(TInput input, TInput storedValue);
}

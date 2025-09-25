namespace ByteLink.Domain.Generators;

public interface IGenerator<TInput, TOutput>
{
    public TOutput Generate(TInput input);
}

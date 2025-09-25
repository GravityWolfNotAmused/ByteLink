namespace ByteLink.Domain.Settings;

public sealed class ByteLinkAuthSettings
{
    public required string Issuer { get; init; }
    public required string Key { get; init; }
}
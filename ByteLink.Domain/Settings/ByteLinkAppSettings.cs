namespace ByteLink.Domain.Settings;

public sealed class ByteLinkAppSettings
{
    public required bool IsHttps { get; set; }
    public required string Domain { get; set; }
}

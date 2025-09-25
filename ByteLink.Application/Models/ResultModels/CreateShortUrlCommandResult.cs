namespace ByteLink.Application.Models.ResultModels;

public class CreateShortUrlCommandResult
{
    public required string SourceUrl { get; set; }
    public required string ShortUrl { get; set; }
}

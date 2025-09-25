namespace ByteLink.Application.Models.ViewModels;

public class UrlTotalVisitViewModel
{
    public required string SourceUrl { get; set; }
    public required string ShortUrl { get; set; }
    public long TotalVisits { get; set; }
}
namespace ByteLink.Application.Models.ViewModels;

public class UrlVisitViewModel
{
    public required string ShortCode { get; set; }
    public required string ShortUrl { get; set; }
    public DateTime ClickedAt { get; set; }
}
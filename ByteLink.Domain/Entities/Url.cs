namespace ByteLink.Domain.Entities;

public class Url
{
    public long Id { get; private set; }
    public List<UrlVisit> Visits { get; set; } = [];
    public long TotalVisits { get; set; }
    public string SourceUrl { get; private set; }
    public string ShortCode { get; private set; }
    public string UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Url()
    {
        SourceUrl = string.Empty;
        ShortCode = string.Empty;
        UserId = string.Empty;
    }

    private Url(string sourceUrl, string shortCode, string userId, DateTime createdAt)
    {
        SourceUrl = sourceUrl;
        ShortCode = shortCode;
        CreatedAt = createdAt;
        UserId    = userId;
    }

    public static Url Create(string originalUrl, string shortCode, string userId)
    {
        return new Url(originalUrl, shortCode, userId, DateTime.UtcNow);
    }
}

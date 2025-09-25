namespace ByteLink.Domain.Entities;

public class UrlVisit
{
    public long Id { get; private set; }
    public long UrlId { get; private set; }
    public Url Url { get; private set; }
    public DateTime ClickedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

#pragma warning disable CS8618 // Used by EF Core
    public UrlVisit()
#pragma warning restore CS8618
    {

    }

    public static UrlVisit Create(long urlId)
    {
        return new()
        {
            UrlId = urlId,
            ClickedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }
}

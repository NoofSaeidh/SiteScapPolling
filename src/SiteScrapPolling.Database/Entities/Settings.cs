namespace SiteScrapPolling.Database.Entities;

public class Settings
{
    public string Id { get; set; } = null!;
    public long UserId { get; set; }

    public byte[] Timestamp { get; set; } = null!;

    public User? User { get; set; }
}

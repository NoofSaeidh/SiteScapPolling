namespace SiteScrapPolling.Database.Entities;

public class User
{
    public long Id { get; set; }
    public string? LastHandler { get; set; }
    public DateTimeOffset LastDate { get; set; }

    public byte[] Timestamp { get; set; } = null!;

    public List<Settings> Settings { get; set; } = new();
}

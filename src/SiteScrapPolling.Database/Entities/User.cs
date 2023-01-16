namespace SiteScrapPolling.Database.Entities;

public class User
{
    public long Id { get; set; }
    public List<Settings> Settings { get; set; } = new();
}

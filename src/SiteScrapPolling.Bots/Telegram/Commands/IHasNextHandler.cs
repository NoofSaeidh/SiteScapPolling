namespace SiteScrapPolling.Bots.Telegram.Commands;

public interface IHasNextHandler
{
    string NextHandler { get; }
}

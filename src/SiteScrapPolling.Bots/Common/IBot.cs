namespace SiteScrapPolling.Bots.Common
{
    public interface IBot
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}

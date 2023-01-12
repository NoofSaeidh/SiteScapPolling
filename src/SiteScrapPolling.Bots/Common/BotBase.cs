using SiteScrapPolling.Scrapping;

namespace SiteScrapPolling.Bots.Common
{
    public abstract class BotBase : IBot
    {
        protected BotBase(ILogger logger, IScrapper scrapper)
        {
            Logger = logger;
            Scrapper = scrapper;
        }

        protected ILogger Logger { get; }
        protected IScrapper Scrapper { get; }

        public abstract string Name { get; }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.Information("Starting bot {Name}", Name);
            try
            {
                await ExecuteAsyncImpl(stoppingToken);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to start bot {Name}", Name);
            }
        }

        protected abstract Task ExecuteAsyncImpl(CancellationToken stoppingToken);
    }
}

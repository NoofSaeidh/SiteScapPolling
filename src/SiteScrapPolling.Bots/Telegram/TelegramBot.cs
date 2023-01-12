using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Scrapping;

namespace SiteScrapPolling.Bots.Telegram
{
    public class TelegramBot : BotBase
    {
        public TelegramBot(ILogger logger, IScrapper scrapper) : base(logger, scrapper)
        {
        }

        public override string Name => "Telegram";
        protected override async Task ExecuteAsyncImpl(CancellationToken stoppingToken)
        {
            Logger.Information("Telegram is started");
            throw new InvalidOperationException("Some unexpected error");
        }
    }
}

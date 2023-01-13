using Microsoft.Extensions.Options;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Scrapping;

namespace SiteScrapPolling.Bots.Telegram
{
    public class TelegramBot : BotBase
    {
        private readonly IOptionsMonitor<TelegramBotOptions> _options;

        public TelegramBot(ILogger logger, IScrapper scrapper, IOptionsMonitor<TelegramBotOptions> options)
            : base(logger, scrapper)
        {
            _options = options;
        }

        public override string Name => "Telegram";

        protected override async Task ExecuteAsyncImpl(CancellationToken stoppingToken)
        {
            Logger.Information("Telegram is started");
            throw new InvalidOperationException("Some unexpected error");
        }
    }
}

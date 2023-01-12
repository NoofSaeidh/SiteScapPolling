using Microsoft.Extensions.Hosting;

namespace SiteScrapPolling.Bots.Common
{
    public class BotService : BackgroundService
    {
        private readonly IReadOnlyCollection<IBot> _bots;
        private readonly ILogger _logger;

        public BotService(IEnumerable<IBot> bots, ILogger logger)
        {
            _bots = bots.ToList();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting bots");
            if (_bots.Count == 0)
            {
                _logger.Warning("No bots found");
                return;
            }

            await Task.WhenAll(_bots.Select(b => b.ExecuteAsync(stoppingToken)));
        }
    }
}

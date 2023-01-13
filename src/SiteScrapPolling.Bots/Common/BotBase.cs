using Serilog.Context;
using SiteScrapPolling.Scrapping;

namespace SiteScrapPolling.Bots.Common;

public abstract class BotBase : IBot
{
    protected BotBase(ILogger logger, IScrapper scrapper)
    {
        Logger   = logger;
        Scrapper = scrapper;
    }

    protected ILogger Logger { get; }
    protected IScrapper Scrapper { get; }

    public abstract string Name { get; }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.Information("Starting bot {BotName}", Name);
        try
        {
            using (LogContext.PushProperty("BotName", Name))
                await ExecuteAsyncImpl(stoppingToken);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to start bot {BotName}", Name);
        }
    }

    protected abstract Task ExecuteAsyncImpl(CancellationToken stoppingToken);
}

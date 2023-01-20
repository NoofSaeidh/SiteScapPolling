using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class HandlerBase
{
    protected HandlerBase(ITelegramBotClient client, ILogger logger)
    {
        Client = client;
        Logger = logger;
    }

    protected ILogger Logger { get; }
    protected ITelegramBotClient Client { get; }


    public abstract bool CanHandle(Update update);


    public virtual async Task<bool> TryHandleAsync(Update update, CancellationToken cancellationToken)
    {
        Logger.Debug("Handling update {@Update} with handler {Handler}", update, this);
        try
        {
            await HandleAsync(update, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to handle update {@Update}", update);
            return false;
        }
    }

    protected abstract Task HandleAsync(Update update, CancellationToken cancellationToken);
}

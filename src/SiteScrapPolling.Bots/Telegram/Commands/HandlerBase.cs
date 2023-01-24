using SiteScrapPolling.Database.Repositories;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class HandlerBase
{
    protected HandlerBase(ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
    {
        Client         = client;
        Logger         = logger;
        UserRepository = userRepository;
    }

    protected ILogger Logger { get; }
    protected ITelegramBotClient Client { get; }
    protected IUserRepository UserRepository { get; }


    public abstract bool CanHandle(Update update);

    protected virtual bool LogCanHandle(Update update, bool canHandle)
    {
        Logger.Debug("Update {@Update} {CanClause} be handled by handler {Handler}",
                     update, canHandle ? "can" : "cannot", this);
        return canHandle;
    }


    public virtual async Task<bool> TryHandleAsync(Update update, CancellationToken cancellationToken)
    {
        Logger.Debug("Handling update {@Update} with handler {Handler}", update, this);
        try
        {
            await HandleAsync(update, cancellationToken);
            await UserRepository.SetLastHandlerAsync(update.GetUserId(), ToString(), cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to handle update {@Update}", update);
            return false;
        }
    }

    protected abstract Task HandleAsync(Update update, CancellationToken cancellationToken);

    public override string ToString()
    {
        return GetType().Name;
    }
}

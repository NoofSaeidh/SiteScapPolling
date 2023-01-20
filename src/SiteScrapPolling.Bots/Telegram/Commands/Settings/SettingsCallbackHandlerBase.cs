using SiteScrapPolling.Database;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands.Settings;

public abstract class SettingsCallbackHandlerBase : CallbackCommandHandlerBase
{
    protected SettingsCallbackHandlerBase(ITelegramBotClient client, ILogger logger, DbContext dbContext)
        : base(client, logger)
    {
        DbContext = dbContext;
    }

    protected DbContext DbContext { get; }
}

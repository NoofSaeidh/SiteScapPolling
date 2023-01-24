using SiteScrapPolling.Database.Repositories;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands.Settings;

public abstract class SettingsCallbackHandlerBase : CallbackCommandHandlerBase
{
    protected SettingsCallbackHandlerBase(ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
        : base(client, logger, userRepository)
    {
    }

}

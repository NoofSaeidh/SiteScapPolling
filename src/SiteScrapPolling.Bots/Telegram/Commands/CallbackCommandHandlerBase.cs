using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class CallbackCommandHandlerBase : HandlerBase
{
    protected CallbackCommandHandlerBase(ITelegramBotClient client, ILogger logger) : base(client, logger)
    {
    }

    public abstract CallbackCommand Command { get; }

    public override bool CanHandle(Update update)
    {
        return false;
    }

}

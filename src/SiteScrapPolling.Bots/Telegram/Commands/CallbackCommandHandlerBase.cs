using Telegram.Bot.Types;
using Telegram.Bot;
using SiteScrapPolling.Database.Repositories;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class CallbackCommandHandlerBase : HandlerBase
{
    protected CallbackCommandHandlerBase(ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
        : base(client, logger, userRepository)
    {
    }

    public abstract CallbackCommand Command { get; }


    public override bool CanHandle(Update update)
    {
        return LogCanHandle(update, update?.CallbackQuery?.Data == Command.CallbackData);
    }

    protected override async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        if (update?.CallbackQuery is { } callback)
        {
            await HandleAsync(callback, update.GetUserId(), cancellationToken);
        }
    }

    protected abstract Task HandleAsync(CallbackQuery callbackQuery, long userId, CancellationToken cancellationToken);
}

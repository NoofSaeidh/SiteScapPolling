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
        return LogCanHandle(update, update?.CallbackQuery?.Data == Command.CallbackData);
    }

    protected override async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        if (update?.CallbackQuery is { } callback)
        {
            await HandleAsync(callback, cancellationToken);
        }
    }

    protected abstract Task HandleAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);

    protected virtual long GetChatId(CallbackQuery callbackQuery) => callbackQuery.Message?.Chat.Id ?? callbackQuery.From.Id;
}

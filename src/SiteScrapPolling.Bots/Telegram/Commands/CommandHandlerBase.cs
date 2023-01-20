using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class CommandHandlerBase : HandlerBase
{
    protected CommandHandlerBase(ITelegramBotClient client, ILogger logger) : base(client, logger)
    {
    }

    public abstract Command Command { get; }


    public override bool CanHandle(Update update)
    {
        var result = update?.Message?.Text == Command.FullCommand;
        Logger.Debug("Update {@Update} {CanClause} be handled by command {Command}",
                     update, result ? "can" : "cannot", Command.Name);
        return result;
    }

    protected override async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        if (update?.Message is { } message)
        {
            await HandleAsync(message, cancellationToken);
        }
    }

    protected abstract Task HandleAsync(Message message, CancellationToken cancellationToken);
}

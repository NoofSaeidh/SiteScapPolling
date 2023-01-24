using SiteScrapPolling.Database;
using SiteScrapPolling.Database.Repositories;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class CommandHandlerBase : HandlerBase
{
    protected CommandHandlerBase(ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
        : base(client, logger, userRepository)
    {
    }

    public abstract Command Command { get; }


    public override bool CanHandle(Update update)
    {
        return LogCanHandle(update, update?.Message?.Text == Command.FullCommand);
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

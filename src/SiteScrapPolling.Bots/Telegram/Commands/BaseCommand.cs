using Telegram.Bot.Types;
using Telegram.Bot;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public abstract class BaseCommand
{
    protected BaseCommand(TelegramBot telegramBot, ILogger logger)
    {
        TelegramBot = telegramBot;
        Logger      = logger;
    }

    protected TelegramBot TelegramBot { get; }
    protected ILogger Logger { get; }
    protected ITelegramBotClient Client => TelegramBot.Client;
    public abstract TelegramBotCommand Command { get; }


    public bool CanHandle(Message message)
    {
        var result = message is { Text: { } text } && text == Command.Command;
        Logger.Debug("Update {@Update} {CanClause} be handled by command {Command}",
                     message, result ? "can" : "cannot", Command.Name);
        return result;
    }


    public async Task<bool> TryHandleAsync(Message message, CancellationToken cancellationToken)
    {
        if (CanHandle(message) is false)
            return false;

        Logger.Debug("Handling update {@Message} with command {Command}", message, Command.Name);
        try
        {
            await HandleAsync(message, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to handle update {@Message}", message);
            return false;
        }
    }

    protected abstract Task HandleAsync(Message message, CancellationToken cancellationToken);
}

using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class SettingsCommand : BaseCommand
{
    public SettingsCommand(TelegramBot telegramBot, ILogger logger) : base(telegramBot, logger)
    {
    }

    public override TelegramBotCommand Command => TelegramBotCommand.Settings;

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "Here this settings: {}.",
                                          cancellationToken: cancellationToken);
    }
}

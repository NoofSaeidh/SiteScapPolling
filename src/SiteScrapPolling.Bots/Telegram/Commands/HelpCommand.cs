using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class HelpCommand : BaseCommand
{
    public HelpCommand(TelegramBot telegramBot, ILogger logger) : base(telegramBot, logger)
    {
    }

    public override TelegramBotCommand Command => TelegramBotCommand.Help;

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "You can use this bot for sites scrapping.",
                                          cancellationToken: cancellationToken);
    }
}

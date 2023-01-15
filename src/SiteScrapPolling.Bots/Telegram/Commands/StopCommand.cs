using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class StopCommand : BaseCommand
{
    public StopCommand(TelegramBot telegramBot, ILogger logger) : base(telegramBot, logger)
    {
    }

    public override TelegramBotCommand Command => TelegramBotCommand.Stop;

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "Site scrapping is stopped.",
                                          cancellationToken: cancellationToken);
    }
}

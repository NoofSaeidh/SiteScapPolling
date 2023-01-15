using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class StartCommand : BaseCommand
{
    public StartCommand(TelegramBot telegramBot, ILogger logger) : base(telegramBot, logger)
    {
    }

    public override TelegramBotCommand Command => TelegramBotCommand.Start;

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "Site scrapping is started.",
                                          cancellationToken: cancellationToken);
    }
}

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class HelpHandler : CommandHandlerBase
{
    public HelpHandler(ITelegramBotClient client, ILogger logger) : base(client, logger)
    {
    }

    public override Command Command { get; } = new("help", "Show help");

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "You can use this bot for sites scrapping.",
                                          cancellationToken: cancellationToken);
    }
}

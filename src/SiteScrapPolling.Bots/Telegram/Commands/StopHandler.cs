using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class StopHandler : CommandHandlerBase
{
    public StopHandler(ITelegramBotClient client, ILogger logger) : base(client, logger)
    {
    }

    public override Command Command { get; } = new("stop", "Stop site scrapping");

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "Site scrapping is stopped.",
                                          cancellationToken: cancellationToken);
    }
}

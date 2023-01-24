using SiteScrapPolling.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class StartHandler : CommandHandlerBase
{
    public StartHandler( ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
        : base(client, logger, userRepository)
    {
    }

    public override Command Command { get; } = new("start", "Start site scrapping");

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "Site scrapping is started.",
                                          cancellationToken: cancellationToken);
    }
}

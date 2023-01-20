using SiteScrapPolling.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands.Settings;

public class AddCallbackHandler : SettingsCallbackHandlerBase
{
    public AddCallbackHandler(ITelegramBotClient client, ILogger logger, DbContext dbContext)
        : base(client, logger, dbContext)
    {
    }

    public override CallbackCommand Command { get; } = new("Add", "settings_add");

    public override bool CanHandle(Update update)
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
    }
}

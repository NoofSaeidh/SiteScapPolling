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
    
    protected override async Task HandleAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await Client.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
        await Client.SendTextMessageAsync(GetChatId(callbackQuery), "Enter site name",
                                          cancellationToken: cancellationToken);
    }
}

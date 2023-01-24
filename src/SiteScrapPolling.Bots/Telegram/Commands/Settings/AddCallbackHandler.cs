using System.Collections.ObjectModel;
using SiteScrapPolling.Database.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands.Settings;

public class AddCallbackHandler : SettingsCallbackHandlerBase, IHasNextHandler
{
    public AddCallbackHandler(ITelegramBotClient client, ILogger logger, IUserRepository userRepository)
        : base(client, logger, userRepository)
    {
    }

    public override CallbackCommand Command { get; } = new("Add", "settings_add");

    public string NextHandler => null!;

    protected override async Task HandleAsync(CallbackQuery callbackQuery,
                                              long userId,
                                              CancellationToken cancellationToken)
    {
        await Client.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);
        await Client.SendTextMessageAsync(userId, "Enter site name", cancellationToken: cancellationToken);
    }
}

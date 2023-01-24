using SiteScrapPolling.Bots.Telegram.Commands.Settings;
using SiteScrapPolling.Database.Repositories;
using System.Collections.ObjectModel;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class SettingsHandler : CommandHandlerBase
{
    private readonly IEnumerable<SettingsCallbackHandlerBase> _handlers;

    public SettingsHandler(ITelegramBotClient client,
                           ILogger logger,
                           IUserRepository userRepository,
                           IEnumerable<SettingsCallbackHandlerBase> handlers)
        : base(client, logger, userRepository)
    {
        _handlers = handlers;
    }

    public override Command Command { get; } = new("settings", "Show settings");
    
    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        await Client.SendTextMessageAsync(message.Chat.Id, "What do you want to change?",
                                          replyMarkup: new InlineKeyboardMarkup(
                                              _handlers.Select(
                                                  h => InlineKeyboardButton.WithCallbackData(
                                                      h.Command.Label, h.Command.CallbackData)).ToArray()),
                                          cancellationToken: cancellationToken);
    }
}

using SiteScrapPolling.Bots.Telegram.Commands.Settings;
using SiteScrapPolling.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class SettingsHandler : CommandHandlerBase
{
    private readonly DbContext _context;
    private readonly IEnumerable<SettingsCallbackHandlerBase> _handlers;

    public SettingsHandler(ITelegramBotClient client, ILogger logger, DbContext context,
                           IEnumerable<SettingsCallbackHandlerBase> handlers)
        : base(client, logger)
    {
        _context  = context;
        _handlers = handlers;
    }

    public override Command Command { get; } = new("settings", "Show settings");

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { message.Chat.Id }, cancellationToken);
        if (user == null)
        {
            var userRes =
                await _context.Users.AddAsync(new Database.Entities.User { Id = message.Chat.Id }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            user = userRes.Entity;
        }

        await Client.SendTextMessageAsync(message.Chat.Id, "What do you want to change?",
                                          replyMarkup: new InlineKeyboardMarkup(
                                              _handlers.Select(
                                                  h => InlineKeyboardButton.WithCallbackData(
                                                      h.Command.Label, h.Command.CallbackData)).ToArray()),
                                          cancellationToken: cancellationToken);
    }
}

using SiteScrapPolling.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public class SettingsCommand : BaseCommand
{
    private readonly DbContext _context;

    public SettingsCommand(TelegramBot telegramBot, ILogger logger, DbContext context) : base(telegramBot, logger)
    {
        _context = context;
    }

    public override TelegramBotCommand Command => TelegramBotCommand.Settings;

    protected override async Task HandleAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] {message.Chat.Id}, cancellationToken);
        if (user == null)
        {
            var userRes =
                await _context.Users.AddAsync(new Database.Entities.User { Id = message.Chat.Id }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            user = userRes.Entity;
        }

        await Client.SendTextMessageAsync(message.Chat.Id, "You don't have.",
                                          cancellationToken: cancellationToken);
    }
}

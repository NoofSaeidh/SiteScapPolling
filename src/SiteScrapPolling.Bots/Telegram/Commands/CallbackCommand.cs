using SiteScrapPolling.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SiteScrapPolling.Bots.Telegram.Commands;

public record struct CallbackCommand(string Label, string CallbackData)
{
}

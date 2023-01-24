using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram;

public static class Extensions
{
    public static long GetUserId(this Update update) => update.Message?.From?.Id
                                                     ?? update.Message?.Chat.Id
                                                     ?? update.EditedMessage?.Chat.Id
                                                     ?? update.CallbackQuery?.From.Id
                                                     ?? throw new InvalidOperationException("Cannot find user id");
}

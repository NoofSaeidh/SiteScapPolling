using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteScrapPolling.Bots.Telegram;

public record TelegramBotCommand(string Command, string Description)
{
    public static TelegramBotCommand Start => new("/start", "Starts the bot");
    public static TelegramBotCommand Stop => new("/stop", "Stops the bot");
    public static TelegramBotCommand Help => new("/help", "Shows this help");
    public static TelegramBotCommand Settings => new("/settings", "Configure the bot");
        
    public static IEnumerable<TelegramBotCommand> All => new[]
    {
        Start,
        Stop,
        Help,
        Settings,
    };
}

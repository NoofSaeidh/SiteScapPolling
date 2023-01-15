using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteScrapPolling.Bots.Telegram;

public record struct TelegramBotCommand(string Name, string Description)
{
    public string Command => '/' + Name;

    public static TelegramBotCommand Start => new("start", "Starts site scrapping.");
    public static TelegramBotCommand Stop => new("stop", "Stops site scrapping.");
    public static TelegramBotCommand Help => new("help", "Shows help.");
    public static TelegramBotCommand Settings => new("settings", "Configure site scrapping.");

    public static IEnumerable<TelegramBotCommand> All { get; } = new ReadOnlyCollection<TelegramBotCommand>(new[]
    {
        Start,
        Stop,
        Help,
        Settings,
    });

    public readonly bool Equals(TelegramBotCommand other)
    {
        return string.Equals(Name, other.Name, StringComparison.InvariantCulture);
    }

    public override string ToString()
    {
        return $"{Command} - {Description}";
    }

    public readonly override int GetHashCode()
    {
        return StringComparer.InvariantCulture.GetHashCode(Name);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SiteScrapPolling.Bots.Telegram;

public class TelegramBotOptions
{
    public string? AccessToken { get; set; }
}

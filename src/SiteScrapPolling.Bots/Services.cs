using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Bots.Telegram;
using SiteScrapPolling.Bots.Telegram.Commands;

namespace SiteScrapPolling.Bots
{
    public static class Services
    {
        public static IServiceCollection RegisterBots(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<TelegramBot>()
                           .AddSingleton<IBot>(s => s.GetRequiredService<TelegramBot>())
                           .AddSingleton<BaseCommand, HelpCommand>()
                           .AddSingleton<BaseCommand, SettingsCommand>()
                           .AddSingleton<BaseCommand, StartCommand>()
                           .AddSingleton<BaseCommand, StopCommand>()
                           .AddSingleton<AllCommands>()
                           .Configure<TelegramBotOptions>(configuration.GetSection("Bots:Telegram"))
                           .AddHostedService<BotService>()
                           .AddHttpClient();
        }

    }
}

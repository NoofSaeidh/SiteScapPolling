using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Bots.Telegram;
using SiteScrapPolling.Bots.Telegram.Commands;
using SiteScrapPolling.Bots.Telegram.Commands.Settings;
using SiteScrapPolling.Common.Extensions;
using Telegram.Bot;

namespace SiteScrapPolling.Bots
{
    public static class Services
    {
        public static IServiceCollection RegisterBots(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<ITelegramBotClient>(s =>
                           {
                               var accessToken = s.GetRequiredService<IOptions<TelegramBotOptions>>().Value.AccessToken;
                               if (string.IsNullOrEmpty(accessToken))
                               {
                                   s.GetRequiredService<ILogger>().Error("No access token provided for Telegram bot");
                                   return new TelegramBotClient(string.Empty);
                               }

                               return new TelegramBotClient(accessToken,
                                                            s.GetRequiredService<IHttpClientFactory>().CreateClient());
                           })
                           .AddSingleton<TelegramBot>()
                           .AddSingleton<IBot>(s => s.GetRequiredService<TelegramBot>())
                           .FromAssembly<CommandHandlerBase>(t => services.AddSingleton(typeof(CommandHandlerBase), t))
                           .FromAssembly<CallbackCommandHandlerBase>(
                               t => services.AddSingleton(typeof(CallbackCommandHandlerBase), t))
                           .Configure<TelegramBotOptions>(configuration.GetSection("Bots:Telegram"))
                           .AddHostedService<BotService>()
                           .AddHttpClient();
        }
    }
}

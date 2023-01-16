using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Debugging;
using SiteScrapPolling.Bots;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Bots.Telegram;
using SiteScrapPolling.Database;
using SiteScrapPolling.Scrapping;
using SiteScrapPolling.Scrapping.Scrappers;

namespace SiteScrapPolling;

internal static class Services
{
    public static void Configure(HostBuilderContext context, IServiceCollection services)
    {
        services.RegisterLogger(context.Configuration)
                .RegisterBots(context.Configuration)
                .RegisterScrapper()
                .RegisterDatabase(context.Configuration);
    }

    private static IServiceCollection RegisterLogger(this IServiceCollection services, IConfiguration configuration)
    {
        const string path = ".\\logs\\";

        Directory.CreateDirectory(path: path);

        Log.Logger = new LoggerConfiguration()
                     .ReadFrom.Configuration(configuration)
                     .CreateLogger();

        var selfLog = File.CreateText(path + "self-log.txt");
        SelfLog.Enable(TextWriter.Synchronized(writer: selfLog));

        return services.AddSingleton(Log.Logger)
                       .AddLogging(config =>
                       {
                           config.ClearProviders();
                           config.AddSerilog(Log.Logger, dispose: true);
                       });
    }
    
    private static IServiceCollection RegisterScrapper(this IServiceCollection services)
    {
        return services.AddSingleton<IScrapper, DefaultScrapper>();
    }
}

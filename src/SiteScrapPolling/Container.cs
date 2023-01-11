using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace SiteScapPolling
{
    internal static class Container
    {
        private static readonly CompactJsonFormatter CompactJsonFormatter = new();

        public static IContainer Register()
        {
            return new ContainerBuilder().RegisterServices().Build();
        }

        private static ContainerBuilder RegisterServices(this ContainerBuilder container)
        {
            return container.RegisterLogger()
                            .RegisterPoller()
                            .RegisterScrapper();
        }

        private static ContainerBuilder RegisterLogger(this ContainerBuilder container)
        {
            const string path = ".\\logs\\";

            Directory.CreateDirectory(path);

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                         .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                         .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning)
                                               .WriteTo.RollingFile(path + "log-warning-{Date}.json"))
                         .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Debug)
                                               .WriteTo.RollingFile(path + "log-debug-{Date}.json"))
                         .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Information)
                                               .WriteTo.RollingFile(path + "log-information-{Date}.json"))
                         .CreateLogger();

            var selfLog = File.CreateText(path + "self-log.txt");
            Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(selfLog));

            container.RegisterInstance(Log.Logger);

            return container;
        }

        private static ContainerBuilder RegisterPoller(this ContainerBuilder container)
        {
            return container;
        }

        private static ContainerBuilder RegisterScrapper(this ContainerBuilder container)
        {
            return container;
        }

        private static LoggerConfiguration RollingFile(this LoggerSinkConfiguration loggerSinkConfiguration,
                                                       string path)
        {
            return loggerSinkConfiguration.File(CompactJsonFormatter, path, shared: true,
                                                rollingInterval: RollingInterval.Hour);
        }
    }
}
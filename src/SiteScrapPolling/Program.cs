using Autofac;
using Serilog;
using SiteScapPolling;

using (var container = Container.Register())
{
    container.Resolve<ILogger>().Information("Started");
}
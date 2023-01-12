using Autofac;
using Microsoft.Extensions.Hosting;
using Serilog;
using SiteScapPolling;


await Host.CreateDefaultBuilder(args)
          .ConfigureServices(Services.Configure)
          .Build()
          .RunAsync();

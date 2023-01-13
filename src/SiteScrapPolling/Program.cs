using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SiteScapPolling;


await Host.CreateDefaultBuilder(args)
          .ConfigureServices(Services.Configure)
          .ConfigureAppConfiguration(config => config.AddUserSecrets<Program>())
          .Build()
          .RunAsync();

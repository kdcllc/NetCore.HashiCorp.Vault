using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.Configuration.HashiCorpVault.Test;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configBuilder) => {
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddJsonFile("appsettings.json", optional: true);
                    configBuilder.AddCommandLine(args);

                    var configuration = configBuilder.Build();

                    #region DEBUG: Seed the Vault before reading into Configurations
                    // bind vault options
                    var options = new VaultOptions();
                    configuration.Bind("VaultOptions", options);

                    // bind seeder
                    var seedData = new List<VaultSeeder>();
                    configuration.Bind("VaultSeeder", seedData);

                    var logger = new LoggerFactory()
                           .AddConsole()
                           .AddDebug()
                           .CreateLogger<VaultWriteService>();
                    // seed
                    new VaultWriteService(
                           logger,
                           options,
                           seedData
                           ).SeedVault();
                    #endregion

                    // retrieve encrypted values and make available to the application
                    configuration = configBuilder.AddHashiCorpVault(configuration).Build();

                    // set configuration
                    context.Configuration = configuration;
                })
                .UseStartup<Startup>();
    }
}

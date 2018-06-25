using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        // Linux SIGTERM
        private static ManualResetEvent _shutdown = new ManualResetEvent(false);
        public static ManualResetEventSlim _complete = new ManualResetEventSlim();
        private static ILogger _logger;

        public static IConfigurationRoot Configuration { get; set; }
        public static bool IsDevelopment {get;set;}
        public static string Enviroment { get; set; }

        static int Main(string[] args)
        {
 
            try
            {
                // create instance of the logger for the program process
                _logger = new LoggerFactory()
                    .AddConsole()
                    .AddDebug()
                    .CreateLogger(nameof(Program));
                _logger.LogInformation("Starting application...");

                #region SIGTERM
                var ended = new ManualResetEventSlim();
                var starting = new ManualResetEventSlim();

                //Console.WriteLine("Starting application...");

                // Capture SIGTERM  
                AssemblyLoadContext.Default.Unloading += Default_Unloading; 
                #endregion

                // Build enviroment type
                BuildEnviromentVariable();

                // Build configurations
                BuildConfiguration();

                // DI services creation
                IServiceCollection services = new ServiceCollection();
                ConfigureServices(services);

                var provider = services.BuildServiceProvider();

                // var mycustom service = provider.GetService<ServiceType>()

                // Enter the appliction.. run it
                provider.GetService<Application>().Run();

                // Wait for a SIGTERM singnal
                _shutdown.WaitOne();
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                _logger.LogError(ex.ToString());
            }
            finally
            {
                // Console.WriteLine("Cleaning up resources");
                _logger.LogInformation("Cleaning up resources");
            }

            // Console.WriteLine("Exiting...");
            _logger.LogInformation("Exiting Appplication...");

            _complete.Set();

            return 0;
        }

        /// <summary>
        /// Dependecy injection in manner of Asp.net application
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureServices(IServiceCollection services)
        {
            #region Add Configurations
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // *If* you need access to generic IConfiguration this is **required**
            services.AddSingleton<IConfiguration>(Configuration);

            // bind object
            var options = new VaultOptions();
            Configuration.Bind("VaultOptions", options);
            services.AddSingleton(options);

            var seedData = new List<VaultSeeder>();
            Configuration.Bind("VaultSeeder", seedData);
            services.AddSingleton(seedData);

            #endregion

            // Add Application dependecies
            services.AddTransient<ISeedVaultService, SeedVaultService>();
            services.AddTransient<IReadVault, ReadVault>();

            #region Add Logging
            // Add build in logging without configuration file
            //services.AddSingleton(new LoggerFactory()
            //    .AddConsole()
            //    .AddDebug());

            // Add build in logging with configuration file
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            }); 
            #endregion

            // Add the actual application
            services.AddTransient<Application>();
        }

        /// <summary>
        /// Configure all of the needed configurations
        /// </summary>
        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                         .AddEnvironmentVariables()
                         .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                         .AddJsonFile("appsettings.json", optional: true);

            if (string.IsNullOrEmpty(Enviroment) && IsDevelopment)
            {
                builder.AddJsonFile("appsettings.development.json", optional: true);
            }
            else
            {
                builder.AddJsonFile($"appsettings.{Enviroment}.json", optional:false);
            }

            Configuration = builder.Build();

            // Add Vault Registration
            Configuration = builder.AddHashiCorpVault(Configuration).Build();
        }

        /// <summary>
        /// Build enviroment variable
        /// </summary>
        private static void BuildEnviromentVariable()
        {
            Enviroment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            IsDevelopment = string.IsNullOrEmpty(Enviroment) ||
                                Enviroment.ToLower() == "development";
        }

        /// <summary>
        /// Terminating the linux process gracefully <see cref="!:http://www.ben-morris.com/using-docker-to-build-and-deploy-net-core-console-applications/"/>
        /// </summary>
        /// <param name="obj"></param>
        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            // Console.WriteLine($"Shutting down in response to SIGTERM.");
            _logger.LogInformation($"Shutting down in response to SIGTERM. {obj.ToString()}");
            _shutdown.Set();
            _complete.Wait();
        }
    }

}

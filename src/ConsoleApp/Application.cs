using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly IReadVault _readVault;

        public Application(ILogger<Application> logger, IReadVault reader)
        {
            _logger = logger;
            _readVault = reader;
        }

        public void Run()
        {
            _logger.LogInformation($"{nameof(Application)} started");

            _readVault.GetValues();
        }
    }
}

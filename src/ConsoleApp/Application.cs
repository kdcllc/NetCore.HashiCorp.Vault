using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly ISeedVaultService _seedVault;
        private readonly IReadVault _readVault;

        public Application(ILogger<Application> logger, ISeedVaultService seedVault, IReadVault reader)
        {
            _logger = logger;
            _seedVault = seedVault;
            _readVault = reader;
        }

        public void Run()
        {
            _logger.LogInformation($"{nameof(Application)} started");

            _seedVault.SeedVault();

            _readVault.GetValues();
        }
    }
}

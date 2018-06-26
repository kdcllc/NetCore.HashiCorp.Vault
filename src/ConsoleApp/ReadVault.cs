using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleApp
{
    public class ReadVault : IReadVault
    {
        private readonly ILogger<ReadVault> _logger;
        private readonly VaultOptions _options;
        private readonly IConfiguration _configuration;

        public ReadVault(ILogger<ReadVault> logger, VaultOptions options, IConfiguration configuration)
        {
            _logger = logger;
            _options = options;
            _configuration = configuration;
        }
        public void GetValues()
        {
            _logger.LogInformation("Vault Reader Started...");

            foreach (var item in _options.Secrets)
            {
                var val = _configuration[item];

                _logger.LogInformation($"Key: {item} {Environment.NewLine} Value: {val}");

            }
           
            _logger.LogInformation("Vault Reader Ended...");
        }
    }
}

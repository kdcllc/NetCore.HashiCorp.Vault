using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ReadVault : IReadVault
    {
        private readonly ILogger<SeedVaultService> _logger;
        private readonly VaultOptions _options;
        private readonly IConfiguration _configuration;

        public ReadVault(ILogger<SeedVaultService> logger, VaultOptions options, IConfiguration configuration)
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

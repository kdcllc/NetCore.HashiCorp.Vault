using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using VaultSharp;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.AppRole;
using VaultSharp.Backends.Authentication.Models.Token;

namespace ConsoleApp
{
    public class SeedVaultService : ISeedVaultService
    {
        private readonly ILogger<SeedVaultService> _logger;
        private readonly VaultOptions _options;
        private readonly List<VaultSeeder> _seeder;

        public SeedVaultService(ILogger<SeedVaultService> logger, VaultOptions options, List<VaultSeeder> seeder)
        {
            _logger = logger;
            _options = options;
            _seeder = seeder;
        }
        public void SeedVault()
        {
            _logger.LogInformation("Starting seeding HashiCopr Vault");

            _logger.LogInformation("Creating Vault client");
            _logger.LogInformation($"Vault Address: {_options.Server}");

            IAuthenticationInfo tokenAuthenticationInfo;

            if (!string.IsNullOrWhiteSpace(_options.TokenId))
            {
                _logger.LogInformation($"Auth Token: {_options.TokenId}");
                tokenAuthenticationInfo = new TokenAuthenticationInfo(_options.TokenId);
            }
            else
            {
                _logger.LogInformation($"AppRole RoleId: {_options.RoleId}");
                _logger.LogInformation($"AppRole SecretId: {_options.SecretId}");

                tokenAuthenticationInfo = new AppRoleAuthenticationInfo("approle", _options.RoleId, _options.SecretId);
            }

            _logger.LogInformation($"Create Vault Client: {_options.Server}");
            var vaultClient = VaultClientFactory.CreateVaultClient(new Uri(_options.Server), tokenAuthenticationInfo);

            foreach (var item in _seeder)
            {
                _logger.LogDebug($"key:{item.key} -- property name: {item.values[0]} -- property value: {item.values[1]}");
                var result = vaultClient.WriteSecretAsync(item.key, new Dictionary<string, object>(){
                        {item.values[0], item.values[1]}
                    }).Result;
                _logger.LogDebug($"Result from Vault Server: {result?.ToString()}");
            }
         }
    }
}

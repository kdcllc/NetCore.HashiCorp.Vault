using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// Extension methods for registering <see cref="HashiCorpVaultConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class HashiCorpVaultConfigurationExtensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationBuilder"/> that reads configuration vaules from the  HashiCorp Vault
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to. </param>
        /// <param name="config">The <see cref="IConfiguration"/> to pass configurations.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddHashiCorpVault(
            this IConfigurationBuilder builder,
            IConfiguration config)
        {

            var options = GetOptions(config);

            var client = new HashiCorpVaultClientWrapper(options);

            builder.Add(new HashiCorpVaultConfigurationSource()
            {
                Client = client,
                Options = options
            });

            return builder;
        }

        private static VaultOptions GetOptions(IConfiguration config)
        {
            var options = new VaultOptions();

            if (config.Get<VaultOptions>().Server == null)
            {
                config.Bind("VaultOptions", options);
            }

            if (string.IsNullOrEmpty(options.Server)) throw new ArgumentNullException(nameof(VaultOptions.Server));

            return options;

        }

    }
}

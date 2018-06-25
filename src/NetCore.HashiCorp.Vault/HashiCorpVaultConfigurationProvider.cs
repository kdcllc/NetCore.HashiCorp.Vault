using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// A HashiCorp Valut provider based on <see cref="ConfigurationProvider"/>
    /// </summary>
    public class HashiCorpVaultConfigurationProvider :  ConfigurationProvider
    {
        private readonly IHashiCorpVaultClient _client;
        private readonly string _basePrefix;
        private readonly IEnumerable<string> _secrets;

        /// <summary>
        /// Creates a new instance of <see cref="HashiCorpVaultConfigurationProvider"/>
        /// </summary>
        /// <param name="client">The <see cref="IHashiCorpVaultClient"/>to use for retrieving values</param>
        /// <param name="keyPrefix"> Prefix for the HashiCorp Vault ie "secrets/mykey_prefix/" or "secret/gourp2/" or "secret/group1/keyC""
        /// </param>
        /// <param name="secrets"> Keys in the HashiCorp Vault keyA, keyB etc. </param>
        public HashiCorpVaultConfigurationProvider(IHashiCorpVaultClient client, string keyPrefix, IEnumerable<string> secrets)
        {
            if (keyPrefix == null)
            {
                throw new ArgumentNullException(nameof(keyPrefix));
            }
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _basePrefix = GetBasePath(keyPrefix);
            _secrets = secrets ?? throw new ArgumentNullException(nameof(secrets));
        }

        /// <inheritdoc/>
        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        /// <summary>
        /// Makes calls thru VaultSharp library
        /// </summary>
        /// <returns></returns>
        internal async Task LoadAsync()
        {
            // making sure that we have the exact casing
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var secretItem in _secrets)
            {
                if (string.IsNullOrWhiteSpace(secretItem))
                {
                    continue;
                }

                try
                {
                    var key = secretItem.Replace('/', ':');
                    var value = await _client.GetSecretAsync($"{_basePrefix}{secretItem}").ConfigureAwait(false);

                    data.Add(key, value);
                }
                catch (Exception ex)
                {
                    // for some keys that are not found in the actual store
                    Trace.WriteLine(ex.ToString());
                }
            }

            // pass the dictionary to the configuration provider
            Data = data;
        }

        /// <summary>
        /// Validates the path to the store
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        internal string GetBasePath(string keyPrefix)
        {
            if (string.IsNullOrWhiteSpace(keyPrefix))
            {
                return string.Empty;
            }

            if (keyPrefix.EndsWith(@"/"))
            {
                return keyPrefix;
            }

            return $"{keyPrefix}/";
        }
    }
}

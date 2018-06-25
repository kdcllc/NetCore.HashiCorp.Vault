namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// Represents HashCorp Vault secrets as an <see cref="IConfigurationSource"/>.
    /// </summary>
    internal class HashiCorpVaultConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Gets or sets the <see cref="VaultOptions"/> to use for retrieving values
        /// </summary>
        public VaultOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IHashiCorpVaultClient"/> instance used to control secret loading.
        /// </summary>
        public IHashiCorpVaultClient Client { get; set; }
        
        /// <inheritdoc />
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new HashiCorpVaultConfigurationProvider(Client, Options.Prefix, Options.Secrets);
        }
    }
}

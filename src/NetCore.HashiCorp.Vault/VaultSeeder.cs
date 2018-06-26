namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// The <see cref="VaultSeeder"/> can be used for seeding the vault with vaules.
    /// </summary>
    public class VaultSeeder
    {
        /// <summary>
        /// Gets or Sets value for the key
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Gets or Sets vaules for the key
        /// </summary>
        public string[] values { get; set; }
    }
}

namespace Microsoft.Extensions.Configuration.HashiCorpVault.Test
{
    /// <summary>
    /// Simple HashiCorp Vault writting tooling. This can be used for the development enviroments to add keys to 
    /// Docker Vault Container.
    /// </summary>
    public interface IVaultWriteService
    {
        /// <summary>
        /// Seeds information from the provided json configuration file.
        /// </summary>
        void SeedVault();
    }
}
